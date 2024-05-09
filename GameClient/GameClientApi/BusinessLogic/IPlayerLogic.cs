using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.BusinessLogic
{
	public interface IPlayerLogic
	{
		bool VerifyLogin(string userName, string password);
		bool CreatePlayer(AccountRegistrationModel newPlayerAccount);
		List<PlayerModel> GetAllPlayersInLobby(int? lobbyId, SqlTransaction transaction = null);
		void UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel, SqlTransaction transaction = null);
		void UpdatePlayerOwnership(PlayerModel player, SqlTransaction transaction = null);
		PlayerModel GetPlayer(string username, SqlTransaction transaction = null);
		bool BanPlayer(string username);
		List<PlayerModel> GetAllPlayers();
	}
}
