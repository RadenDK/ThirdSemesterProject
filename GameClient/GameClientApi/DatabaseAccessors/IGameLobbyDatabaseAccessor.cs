using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GameClientApi.DatabaseAccessors
{
	public interface IGameLobbyDatabaseAccessor
	{
		List<GameLobbyModel> GetAllGameLobbies ();


		bool DeleteGameLobby(int? gameLobbyId);

		int CreateGameLobby(GameLobbyModel gameLobby, SqlTransaction transaction = null);

		int CreateLobbyChat();

		GameLobbyModel GetGameLobby(int gameLobbyId);

		SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

		void CommitTransaction(SqlTransaction sqlTransaction);

		void RollbackTransaction(SqlTransaction sqlTransaction);

    }
}
