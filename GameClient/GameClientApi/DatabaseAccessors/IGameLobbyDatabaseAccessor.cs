using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
	public interface IGameLobbyDatabaseAccessor
	{
		List<GameLobbyModel> GetAllGameLobbies ();


		bool DeleteGameLobby(int? gameLobbyId);

		int CreateGameLobby(GameLobbyModel gameLobby, SqlTransaction transaction);

		int CreateLobbyChat(SqlTransaction transaction);

		GameLobbyModel GetGameLobby(int gameLobbyId);
	}
}
