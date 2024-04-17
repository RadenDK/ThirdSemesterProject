using GameClientApi.Models;

namespace GameClientApi.DatabaseAccessors
{
	public interface IGameLobbyDatabaseAccessor
	{
		List<GameLobbyModel> GetAllGameLobbies ();


		bool DeleteGameLobby(int gameLobbyId);
	}
}
