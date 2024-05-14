using Dapper;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;
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
			PlayerModel player = _playerAccessor.GetPlayer(userName);

			string? storedHashedPassword = null;

			if (player != null)
			{
				storedHashedPassword = player.PasswordHash;
			}

			if (storedHashedPassword == null || userName == null)
			{
				throw new ArgumentNullException("Stored HashedPassword or username is null");
			}
			if (player.Banned == true)
			{
				throw new ArgumentException("Player is banned");
			}
			player.OnlineStatus = true;
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
			if (AccountHasValues(newPlayerAccount))
			{
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
			return false;
		}

		private bool AccountHasValues(AccountRegistrationModel newAccount)
		{
			if (newAccount == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.Username) || _playerAccessor.UsernameExists(newAccount.Username))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.Password))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.Email))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.InGameName) || _playerAccessor.InGameNameExists(newAccount.InGameName))
			{
				return false;
			}
			if (newAccount.BirthDay == null || newAccount.BirthDay < new DateTime(1753, 1, 1) || newAccount.BirthDay > new DateTime(9999, 12, 31))
			{
				return false;
			}

			return true;
		}

		public List<PlayerModel> GetAllPlayersInLobby(int? lobbyId, SqlTransaction transaction = null)
		{
			return _playerAccessor.GetAllPlayersInLobby(lobbyId, transaction);

		}

		public void UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel, SqlTransaction transaction = null)
		{
			if (transaction == null)
			{
				transaction = _playerAccessor.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
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
			else
			{
				_playerAccessor.UpdatePlayerLobbyId(player, newGameLobbyModel, transaction);
			}
		}

		private bool TooManyPlayersInLobby(GameLobbyModel gameLobby, SqlTransaction transaction)
		{
			List<PlayerModel> playersInGameLobby = GetAllPlayersInLobby(gameLobby.GameLobbyId, transaction);

			bool lobbyHasTooManyPlayers = playersInGameLobby.Count > gameLobby.AmountOfPlayers;

			return lobbyHasTooManyPlayers;

		}

		public void UpdatePlayerOwnership(PlayerModel player, SqlTransaction transaction = null)
		{

			_playerAccessor.UpdatePlayerOwnership(player, transaction);

		}

		public List<PlayerModel> GetAllPlayers()
		{
			List<PlayerModel> players = _playerAccessor.GetAllPlayers();
			return players;
		}

		public bool UpdatePlayer(PlayerModel updatedPlayer)
		{
			PlayerModel oldPlayer = _playerAccessor.GetPlayer(playerId: updatedPlayer.PlayerId);

			SqlTransaction transaction = _playerAccessor.BeginTransaction();

			try
			{
				if (UpdatedPlayerHasValues(updatedPlayer, oldPlayer))
				{
					if (updatedPlayer.Banned == true && oldPlayer.Banned == false)
					{
						BanPlayer(updatedPlayer, transaction);
					}
					if (_playerAccessor.UpdatePlayer(updatedPlayer, transaction))
					{
						_playerAccessor.CommitTransaction(transaction);
					}
					return true;
				}
			}
			catch
			{
				_playerAccessor.RollbackTransaction(transaction);
			}
			return false;
		}

		private bool UpdatedPlayerHasValues(PlayerModel updatedPlayer, PlayerModel oldPlayer)
		{
			if (updatedPlayer == null)
			{
				return false;
			}
			if (oldPlayer.Username != updatedPlayer.Username || string.IsNullOrEmpty(updatedPlayer.Username))
			{
				if (_playerAccessor.UsernameExists(updatedPlayer.Username))
				{
					return false;
				}
			}
			if (oldPlayer.InGameName != updatedPlayer.InGameName || string.IsNullOrEmpty(updatedPlayer.InGameName))
			{
				if (_playerAccessor.InGameNameExists(updatedPlayer.InGameName))
				{
					return false;
				}
			}
			if (string.IsNullOrEmpty(updatedPlayer.Email))
			{
				return false;
			}
			if (updatedPlayer.Elo < 0)
			{
				return false;
			}
			if (updatedPlayer.CurrencyAmount < 0)
			{
				return false;
			}
			return true;
		}

		public bool BanPlayer(PlayerModel player, SqlTransaction transaction = null)
		{
			try
			{
				if (player.GameLobbyId != null)
				{
					GameLobbyModel emptyGameLobbyModel = new GameLobbyModel { GameLobbyId = null };
					UpdatePlayerLobbyId(player, emptyGameLobbyModel, transaction);
					if (player.IsOwner)
					{
						player.IsOwner = false;
						UpdatePlayerOwnership(player, transaction);
					}
				}
				if (player.OnlineStatus)
				{
					player.OnlineStatus = false;
					_playerAccessor.SetOnlineStatus(player, transaction);
				}
				return true;
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to ban player: " + ex.Message);
			}
		}
        public bool DeletePlayer(int? playerId)
        {
            return _playerAccessor.DeletePlayer(playerId);
        }
    }
}

