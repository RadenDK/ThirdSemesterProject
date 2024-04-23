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

        public int CreateLobbyChat()
        {
            string createLobbyQuery = "INSERT INTO Chat (CreatedDate, ChatType) OUTPUT INSERTED.ChatID VALUES (@CreatedDate, @ChatType)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                int chatId = connection.QuerySingle<int>(createLobbyQuery, new { CreatedDate = DateTime.UtcNow, ChatType = "Lobby" });
                return chatId;
            }
        }

        public int CreateGameLobby(GameLobbyModel gameLobby)
        {
            int lobbyChatId = CreateLobbyChat();

            string createGameLobbyQuery = "INSERT INTO GameLobby (LobbyName, AmountOfPlayers, PasswordHash, InviteLink, LobbyChatId) OUTPUT INSERTED.GameLobbyID VALUES (@LobbyName, @AmountOfPlayers, @PasswordHash, @InviteLink, @LobbyChatId)";

            try
            {
				if (GameLobbyHasValues(gameLobby))
				{
					using (SqlConnection connection = new SqlConnection(_connectionString))
					{
						connection.Open();
						int gameLobbyId = connection.QuerySingle<int>(createGameLobbyQuery, new { gameLobby.LobbyName, gameLobby.AmountOfPlayers, gameLobby.PasswordHash, gameLobby.InviteLink, LobbyChatId = lobbyChatId });
						return gameLobbyId;
					}
				}
				return 0;
			}
            catch (Exception ex)
            {
                throw new Exception("Not saved in database", ex);
            }
        }

        private bool GameLobbyHasValues(GameLobbyModel gameLobby)
        {
            if (gameLobby == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(gameLobby.LobbyName))
            {
                return false;
            }
            if (gameLobby.AmountOfPlayers < 2 || gameLobby.AmountOfPlayers > 10)
            {
                return false;
            }
            if (string.IsNullOrEmpty(gameLobby.InviteLink))
            {
                return false;
            }

            return true;
        }
    }
}
