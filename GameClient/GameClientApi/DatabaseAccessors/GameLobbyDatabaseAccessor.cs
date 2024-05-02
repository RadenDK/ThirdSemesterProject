using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;

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

        public int CreateGameLobby(GameLobbyModel gameLobby, SqlTransaction transaction = null)
        {
            int lobbyChatId = CreateLobbyChat();

            string createGameLobbyQuery = "INSERT INTO GameLobby (LobbyName, AmountOfPlayers, PasswordHash, InviteLink, LobbyChatId) OUTPUT INSERTED.GameLobbyID VALUES (@LobbyName, @AmountOfPlayers, @PasswordHash, @InviteLink, @LobbyChatId)";

            IDbConnection connection;
            if (transaction != null)
            {
                connection = transaction.Connection;
            }
            else
            {
                connection = new SqlConnection(_connectionString);
                connection.Open();
            }

            if (GameLobbyHasValues(gameLobby))
            {
                int gameLobbyId = connection.QuerySingle<int>(createGameLobbyQuery, new { gameLobby.LobbyName, gameLobby.AmountOfPlayers, gameLobby.PasswordHash, gameLobby.InviteLink, LobbyChatId = lobbyChatId }, transaction: transaction);
                return gameLobbyId;
            }

            if (transaction == null)
            {
                connection.Close();
            }

            return 0;

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
            if (gameLobby.AmountOfPlayers < 2 || gameLobby.AmountOfPlayers > 10) // when a game lobby is created, what is the amount of players in lobby? is it ever more than 1 or 0? 
            {
                return false;
            }
            if (string.IsNullOrEmpty(gameLobby.InviteLink))
            {
                return false;
            }
            return true;
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

        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction(isolationLevel);
            return transaction;
        }


        public void CommitTransaction(SqlTransaction sqlTransaction)
        {
            if (sqlTransaction == null)
            {
                throw new ArgumentNullException(nameof(sqlTransaction));
            }

            try
            {
                sqlTransaction.Commit();
            }
            finally
            {
                if(sqlTransaction.Connection != null)
                {
                    sqlTransaction.Connection.Close();
                } 
            }
        }

        public void RollbackTransaction(SqlTransaction sqlTransaction)
        {
            try
            {
                sqlTransaction.Rollback();
            }
            finally
            {
                try
                {
                    sqlTransaction.Connection.Close();
                }
                catch (NullReferenceException ex)
                {

                }
            }
        }

    }
}
