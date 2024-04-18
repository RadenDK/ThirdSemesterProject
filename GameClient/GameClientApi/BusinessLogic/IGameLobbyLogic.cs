using GameClientApi.Models;

namespace GameClientApi.BusinessLogic
{
	public interface IGameLobbyLogic
	{
		IEnumerable<GameLobbyModel> GetAllGameLobbies();
	}
}
