using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
	public class GameLobbyDatabaseAccessor : IGameLobbyDatabaseAccessor
	{
		private readonly string _connectionString;
		public GameLobbyDatabaseAccessor(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public List<GameLobbyModel> GetAllGameLobbies()
		{
			List<GameLobbyModel> gameLobbies = new List<GameLobbyModel>();
			string getAllGameLobbies = "SELECT GameLobbyID, LobbyName, AmountOfPlayers, PasswordHash, InviteLink FROM GameLobby";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				gameLobbies = connection.Query<GameLobbyModel>(getAllGameLobbies).ToList();
			}
			return gameLobbies;
		}

		public bool DeleteGameLobby(int? gameLobbyId)
		{
			bool deletionSucces = false;

			string deleteLobbyQuery = "DELETE FROM GameLobby WHERE GameLobbyId = @GameLobbyId";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				int rowsAffected = connection.Execute(deleteLobbyQuery, new { GameLobbyId = gameLobbyId });
				if (rowsAffected > 0) deletionSucces = true;
			}

			return deletionSucces;
		}

		public GameLobbyModel GetGameLobby(int gameLobbyId)
		{
			GameLobbyModel gameLobby = null;
			
			string selectLobbyQuery = "SELECT GameLobbyId, LobbyName, AmountOfPlayers, PasswordHash, InviteLink, LobbyChatId " +
				"FROM Gamelobby WHERE GameLobbyId = @GameLobbyId";

			string selectChatQuery = "SELECT ChatId as LobbyChatId, ChatType FROM Chat WHERE ChatId = @ChatId";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var gameLobbyResult = connection.QueryFirstOrDefault<dynamic>(selectLobbyQuery, new { GameLobbyId = gameLobbyId });

				if (gameLobbyResult != null)
				{

					gameLobby = new GameLobbyModel
					{
						GameLobbyId = gameLobbyResult.GameLobbyId,
						LobbyName = gameLobbyResult.LobbyName,
						AmountOfPlayers = gameLobbyResult.AmountOfPlayers,
						PasswordHash = gameLobbyResult.PasswordHash,
						InviteLink = gameLobbyResult.InviteLink
					};

					int? lobbyChatId = gameLobbyResult.LobbyChatId as int?;
					if (lobbyChatId.HasValue)
					{
						gameLobby.LobbyChat = connection.QueryFirstOrDefault<LobbyChatModel>(selectChatQuery, new { ChatId = lobbyChatId });
					}
				}
				return gameLobby;
			}
		}


		public bool CreateGameLobby(GameLobbyModel gameLobbyModel)
		{
			throw new NotImplementedException();
		}
	}
}
