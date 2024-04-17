using GameClientApi.Models;
using System.Collections.Generic;

namespace GameClientApi.Services
{
	public interface IPlayerService
	{
		bool VerifyLogin(string userName, string password);
		bool CreatePlayer(AccountRegistrationModel newPlayerAccount);
		List<PlayerModel> GetAllPlayersInLobby(int lobbyId);
		void UpdatePlayerLobbyId(PlayerModel player);
		void UpdatePlayerOwnership(PlayerModel player);
	}
}
