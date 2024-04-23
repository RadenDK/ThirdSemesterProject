using GameClientApi.Models;

namespace GameClientApi.BusinessLogic
{
	public interface IGameLobbyLogic
	{
		IEnumerable<GameLobbyModel> GetAllGameLobbies();

		GameLobbyModel CreateGameLobby(GameLobbyModel gameLobby, string username);
	}
}
