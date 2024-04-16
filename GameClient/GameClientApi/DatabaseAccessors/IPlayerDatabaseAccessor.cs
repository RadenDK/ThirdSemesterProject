using GameClientApi.Models;

namespace GameClientApi.DatabaseAccessors
{
	public interface IPlayerDatabaseAccessor
	{

		string GetPassword(string userName);

		bool CreatePlayer(AccountRegistrationModel newPlayer);

		bool UsernameExists(string username);

		bool InGameNameExists(string ingamename);
		
		List<PlayerModel> GetAllPlayersInLobby(int lobbyID);

		bool UpdatePlayerLobbyId(PlayerModel player);

		bool UpdatePlayerOwnership(PlayerModel player);

		PlayerModel GetPlayer(string userName);
	}
}
