using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.BusinessLogic
{
	public interface IPlayerLogic
	{
		bool VerifyLogin(string userName, string password);
		bool CreatePlayer(AccountRegistrationModel newPlayerAccount);
		List<PlayerModel> GetAllPlayersInLobby(int? lobbyId, SqlTransaction transaction = null);
		void UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel);
		void UpdatePlayerOwnership(PlayerModel player);
		PlayerModel GetPlayer(string username);
	}
}
