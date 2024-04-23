using GameClientApi.Models;

namespace GameClientApi.BusinessLogic
{
	public interface IGameLobbyLogic
	{
		IEnumerable<GameLobbyModel> GetAllGameLobbies();

		GameLobbyModel JoinGameLobby(int playerId, int gameLobbyId, string password);
	}
}
