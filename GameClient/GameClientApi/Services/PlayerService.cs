using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using BC = BCrypt.Net.BCrypt;
namespace GameClientApi.Services
{
	public class PlayerService : IPlayerService
	{
		IPlayerDatabaseAccessor _playerAccessor;

		public PlayerService(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor)
		{
			_playerAccessor = playerDatabaseAccessor;
		}

		public bool VerifyLogin(string userName, string password)
		{
			string storedHashedPassword = _playerAccessor.GetPassword(userName);
			if (storedHashedPassword == null)
			{
				return false;
			}
			return BC.Verify(password, storedHashedPassword);
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

		public List<PlayerModel> GetAllPlayersInLobby(int lobbyId)
		{
			throw new NotImplementedException();

		}

		public void UpdatePlayerLobbyId(PlayerModel player)
		{
			throw new NotImplementedException();
		}

		public void UpdatePlayerOwnership(PlayerModel player)
		{
			throw new NotImplementedException();

		}


	}
}
