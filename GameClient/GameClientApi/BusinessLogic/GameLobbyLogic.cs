using System.Reflection.Metadata.Ecma335;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using BC = BCrypt.Net.BCrypt;

namespace GameClientApi.BusinessLogic
{
    public class GameLobbyLogic : IGameLobbyLogic
    {
        private readonly IGameLobbyDatabaseAccessor _gameLobbyAccessor;
        private readonly IPlayerLogic _playerLogic;

        public GameLobbyLogic(IConfiguration configuration,
            IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor,
            IPlayerLogic playerLogic)
        {
            _gameLobbyAccessor = gameLobbyDatabaseAccessor;
            _playerLogic = playerLogic;
        }

        public IEnumerable<GameLobbyModel> GetAllGameLobbies()
        {
            List<GameLobbyModel> gameLobbies = _gameLobbyAccessor.GetAllGameLobbies();
            List<GameLobbyModel> validGameLobbies = new List<GameLobbyModel>();

            foreach (GameLobbyModel gameLobby in gameLobbies)

            {
                GameLobbyModel initializeGameLobbyModel = InitializeAndValidateGameLobby(gameLobby);
                if (initializeGameLobbyModel != null)
                {
                    validGameLobbies.Add(initializeGameLobbyModel);
                }
            }

            return validGameLobbies;
        }

        private GameLobbyModel InitializeAndValidateGameLobby(GameLobbyModel gameLobby)
        {
            gameLobby.PlayersInLobby = _playerLogic.GetAllPlayersInLobby(gameLobby.GameLobbyId);

            if (IsValidGameLobby(gameLobby))
            {
                return gameLobby;
            }

            return MakeGameLobbyValid(gameLobby);
        }

        private bool IsValidGameLobby(GameLobbyModel gameLobby)
        {
            return HasPlayers(gameLobby) &&
                   HasSingleOwner(gameLobby) &&
                   HasValidPlayerCount(gameLobby);
        }

        private bool HasPlayers(GameLobbyModel gameLobby)
        {
            return gameLobby.PlayersInLobby.Any();
        }

        private bool HasSingleOwner(GameLobbyModel gameLobby)
        {
            return gameLobby.PlayersInLobby.Count(player => player.IsOwner) == 1;
        }

        private bool HasValidPlayerCount(GameLobbyModel gameLobby)
        {
            return gameLobby.PlayersInLobby.Count <= gameLobby.AmountOfPlayers;
        }

        private GameLobbyModel MakeGameLobbyValid(GameLobbyModel gameLobby)
        {
            if (!HasPlayers(gameLobby))
            {
                _gameLobbyAccessor.DeleteGameLobby(gameLobby.GameLobbyId);
                return null;
            }

            if (!HasSingleOwner(gameLobby))
            {
                AssignOwnerToLobbyAndUpdateDatabase(gameLobby);
            }

            if (HasTooManyOwners(gameLobby))
            {
                RemoveOwnersUntilOneIsLeftAndUpdateDatabase(gameLobby);
            }

            if (!HasValidPlayerCount(gameLobby))
            {
                gameLobby.GameLobbyId = null;
                KickPlayersUntilLobbyCapacityIsMet(gameLobby);
            }

            return gameLobby;
        }

        private bool HasTooManyOwners(GameLobbyModel gameLobby)
        {
            return gameLobby.PlayersInLobby.Count(player => player.IsOwner) > 1;
        }

        private void AssignOwnerToLobbyAndUpdateDatabase(GameLobbyModel gameLobby, SqlTransaction transaction = null)
        {
            PlayerModel firstPlayer = gameLobby.PlayersInLobby.First();
            firstPlayer.IsOwner = true;
            _playerLogic.UpdatePlayerOwnership(firstPlayer, transaction);
        }

        private void KickPlayersUntilLobbyCapacityIsMet(GameLobbyModel gameLobby)
        {
            List<PlayerModel> playersToKick = gameLobby.PlayersInLobby
                .Where(player => !player.IsOwner)
                .Take(gameLobby.PlayersInLobby.Count - gameLobby.AmountOfPlayers)
                .ToList();

            foreach (PlayerModel player in playersToKick)
            {
                gameLobby.PlayersInLobby.Remove(player);
                _playerLogic.UpdatePlayerLobbyId(player, gameLobby);
            }
        }

        private void RemoveOwnersUntilOneIsLeftAndUpdateDatabase(GameLobbyModel gameLobby)
        {
            List<PlayerModel> owners = gameLobby.PlayersInLobby.Where(player => player.IsOwner).Skip(1).ToList();
            foreach (PlayerModel owner in owners)
            {
                owner.IsOwner = false;
                _playerLogic.UpdatePlayerOwnership(owner);
            }
        }

        public GameLobbyModel CreateGameLobby(GameLobbyModel gameLobby, string username)
        {
            SqlTransaction transaction = null;
            try
            {
                if (!string.IsNullOrEmpty(gameLobby.PasswordHash))
                {
                    gameLobby.PasswordHash = HashPassword(gameLobby.PasswordHash);
                }

                gameLobby.InviteLink = GenerateInviteLink();

				transaction = _gameLobbyAccessor.BeginTransaction();

                int gameLobbyId = _gameLobbyAccessor.CreateGameLobby(gameLobby, transaction);

                if (gameLobbyId > 0)
                {
                    gameLobby.GameLobbyId = gameLobbyId;

					PlayerModel player = _playerLogic.GetPlayer(username, transaction);

					player.GameLobbyId = gameLobbyId;
                    _playerLogic.UpdatePlayerLobbyId(player, gameLobby, transaction);

                    player.IsOwner = true;
                    _playerLogic.UpdatePlayerOwnership(player, transaction);

                    _gameLobbyAccessor.CommitTransaction(transaction);

                    return gameLobby;
                }
                return null;
            }
            catch (Exception ex)
            {
                _gameLobbyAccessor.RollbackTransaction(transaction);
                throw new Exception("Failed to create game lobby", ex);
            }
        }



        private string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }


        //This method will be changed later when InviteLink is implemented
        private string GenerateInviteLink()
        {
            int length = 10;
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public GameLobbyModel JoinGameLobby(int playerId, int gameLobbyId, string password)
        {
            GameLobbyModel gameLobby = _gameLobbyAccessor.GetGameLobby(gameLobbyId);

            if (gameLobby == null)
            {
                throw new ArgumentException("Game lobby not found");
            }

            InitializeAndValidateGameLobby(gameLobby);

            if (!string.IsNullOrEmpty(gameLobby.PasswordHash) && !BC.Verify(password, gameLobby.PasswordHash))
            {
                throw new UnauthorizedAccessException("Password does not match");
            }


            if (gameLobby.PlayersInLobby.Count >= gameLobby.AmountOfPlayers)
            {
                throw new ArgumentException("Too many players in lobby");
            }

            PlayerModel player = new PlayerModel { PlayerId = playerId, IsOwner = false };

            _playerLogic.UpdatePlayerLobbyId(player, gameLobby);

            gameLobby.PlayersInLobby = _playerLogic.GetAllPlayersInLobby(gameLobby.GameLobbyId);

            return gameLobby;
        }

        //Opdatere spillers gamelobby id og ejerskabsstatus
        // check om der er 0 spillere i game lobby og hvis ja - slet game lobby
        // check om spilleren er owner og hvis ja - så opdatere owner

        public bool LeaveGameLobby(LeaveGameLobbyRequestModel leaveRequestModel)
        {
            bool playersLeftSuccess = false;

            SqlTransaction transaction = null;
            try
            {
                transaction = _gameLobbyAccessor.BeginTransaction();

                PlayerModel player = new PlayerModel { PlayerId = leaveRequestModel.PlayerId, IsOwner = false };
                GameLobbyModel gameLobby = new GameLobbyModel { GameLobbyId = null };
                _playerLogic.UpdatePlayerLobbyId(player, gameLobby, transaction);

                gameLobby.PlayersInLobby =
                    _playerLogic.GetAllPlayersInLobby(leaveRequestModel.GameLobbyId, transaction);

                if (!HasPlayers(gameLobby))
                {
                    _gameLobbyAccessor.DeleteGameLobby(leaveRequestModel.GameLobbyId, transaction);
                }
                else if (!HasSingleOwner(gameLobby))
                {
                    AssignOwnerToLobbyAndUpdateDatabase(gameLobby, transaction);
                }

                _gameLobbyAccessor.CommitTransaction(transaction);
                playersLeftSuccess = true;
            } 
            catch (Exception ex)
            {
                _gameLobbyAccessor.RollbackTransaction(transaction);
            }
            return playersLeftSuccess;
        } 


    }
}
