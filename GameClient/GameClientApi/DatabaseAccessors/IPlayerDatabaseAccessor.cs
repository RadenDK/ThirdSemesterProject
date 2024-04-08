using GameClientApi.Models;

namespace GameClientApi.DatabaseAccessors
{
	public interface IPlayerDatabaseAccessor
	{
		bool VerifyLogin(string userName, string password);

		string GetPassword(string password);

		bool CreatePlayer(Player newPlayer);

		bool UserNameExists(string username);

		bool InGameNameExists(string ingamename)


	}
}
