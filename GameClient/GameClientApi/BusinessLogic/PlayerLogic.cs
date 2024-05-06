using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using BC = BCrypt.Net.BCrypt;
namespace GameClientApi.BusinessLogic
{
    public class PlayerLogic : IPlayerLogic
    {
        IPlayerDatabaseAccessor _playerAccessor;

        public PlayerLogic(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor)
        {
            _playerAccessor = playerDatabaseAccessor;
        }

        public bool VerifyLogin(string userName, string password)
        {
            string? storedHashedPassword = _playerAccessor.GetPassword(userName);
            PlayerModel player = _playerAccessor.GetPlayer(userName);

            if (storedHashedPassword == null || userName == null)
            {
                throw new ArgumentNullException("Stored HashedPassword or username is null");
            }
            if (player.Banned == true)
            {
				throw new ArgumentException("Player is banned");
			}
            _playerAccessor.SetOnlineStatus(player);
            return BC.Verify(password, storedHashedPassword);
        }

        public PlayerModel GetPlayer(string username, SqlTransaction transaction = null)
        {
            PlayerModel playerData = _playerAccessor.GetPlayer(username);
            if (playerData == null)
            {
                throw new Exception("Player not found");
            }
            return playerData;
        }


        public bool CreatePlayer(AccountRegistrationModel newPlayerAccount)
        {
            if (_playerAccessor.UsernameExists(newPlayerAccount.Username))
            {
                throw new ArgumentException("Username already exists");
            }
            if (_playerAccessor.InGameNameExists(newPlayerAccount.InGameName))
            {
                throw new ArgumentException("InGameName already exists");
            }

            AccountRegistrationModel newPlayerAccountWithHashedPassword = new AccountRegistrationModel
            {
                Username = newPlayerAccount.Username,
                Password = BC.HashPassword(newPlayerAccount.Password),
                Email = newPlayerAccount.Email,
                InGameName = newPlayerAccount.InGameName,
                BirthDay = newPlayerAccount.BirthDay
            };

            return _playerAccessor.CreatePlayer(newPlayerAccountWithHashedPassword);
        }

        public List<PlayerModel> GetAllPlayersInLobby(int? lobbyId, SqlTransaction transaction = null)
        {
            return _playerAccessor.GetAllPlayersInLobby(lobbyId, transaction);

        }

        public void UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel)
        {
            SqlTransaction transaction = _playerAccessor.BeginTransaction(IsolationLevel.ReadUncommitted);

            _playerAccessor.UpdatePlayerLobbyId(player, newGameLobbyModel, transaction);

            if (!TooManyPlayersInLobby(newGameLobbyModel, transaction))
            {
                _playerAccessor.CommitTransaction(transaction);

            }
            else
            {
                _playerAccessor.RollbackTransaction(transaction);
                throw new ArgumentException("Too many players in lobby");
            }
        }

        public void UpdatePlayerLobbyIdCreateGameLobby(PlayerModel player, GameLobbyModel? newGameLobbyModel, SqlTransaction transaction = null)
        {
            if (transaction == null)
            {
                transaction = _playerAccessor.BeginTransaction(IsolationLevel.ReadUncommitted);
            }

            _playerAccessor.UpdatePlayerLobbyId(player, newGameLobbyModel, transaction);

        }

        private bool TooManyPlayersInLobby(GameLobbyModel gameLobby, SqlTransaction transaction)
        {
            List<PlayerModel> playersInGameLobby = GetAllPlayersInLobby(gameLobby.GameLobbyId, transaction);

            bool lobbyHasTooManyPlayers = playersInGameLobby.Count > gameLobby.AmountOfPlayers;

            return lobbyHasTooManyPlayers;

        }

        public void UpdatePlayerOwnership(PlayerModel player, SqlTransaction transaction = null)
        {
            if (transaction == null)
            {
                transaction = _playerAccessor.BeginTransaction(IsolationLevel.ReadUncommitted);
            }
            _playerAccessor.UpdatePlayerOwnership(player, transaction);

        }

        public List<PlayerModel> GetAllPlayers()
        {
            List<PlayerModel> players = _playerAccessor.GetAllPlayers();
            return players;
        }

        public bool BanPlayer(string username)
        {
            SqlTransaction transaction = _playerAccessor.BeginTransaction(IsolationLevel.ReadUncommitted);

            try
            {
                PlayerModel player = GetPlayer(username, transaction);

                player.Banned = true;

                if(player.GameLobbyId != null)
                {
                    GameLobbyModel emptyGameLobbyModel = new GameLobbyModel { GameLobbyId = null };
                    UpdatePlayerLobbyIdCreateGameLobby(player, emptyGameLobbyModel, transaction);
                    if(player.IsOwner)
                    {
                        player.IsOwner = false;
                        UpdatePlayerOwnership(player, transaction);
                    }
                }
                if (player.OnlineStatus) 
                {
                    _playerAccessor.SetOfflineStatus(player, transaction);
                }

                if(!_playerAccessor.BanPlayer(player, transaction))
                {
                    return false;
                }
                _playerAccessor.CommitTransaction(transaction);
                return true;

            }
            catch 
            {
                _playerAccessor.RollbackTransaction(transaction);
                return false;
            }
        }
    }
}
