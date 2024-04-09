using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
    public class PlayerDatabaseAccessor : IPlayerDatabaseAccessor
    {

        private readonly string _connectionString;

        public PlayerDatabaseAccessor(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public bool VerifyLogin(string userName, string password)
        {

            string selectQueryString = "SELECT FROM Player WHERE Username = @UserName AND passwordHash = @Password";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var players = connection.Query<Player>(selectQueryString, new { UserName = userName, Password = password });
                return players != null;
            }
        }

        public string GetPassword(string userName)
        {
            string selectQueryString = "SELECT PasswordHash FROM Player WHERE Username = @UserName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var password = connection.QuerySingleOrDefault<string>(selectQueryString, new { UserName = userName });
                return password;
            }
        }

        public bool CreatePlayer(AccountRegistrationModel newPlayer)
        {
            string insertQuery = "INSERT INTO Player (Username, PasswordHash, InGameName, Email, Birthday) " +
                "VALUES (@Username, @PasswordHash, @InGameName, @Email, @Birthday)";

            bool playerInserted = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var rowsAffected = connection.Execute(insertQuery, newPlayer);
                playerInserted = rowsAffected == 1;
            }
            return playerInserted;

        }

        public bool UserNameExists(string username)
        {
            throw new NotImplementedException();
        }

        public bool InGameNameExists(string ingamename)
        {
            throw new NotImplementedException();
        }

    }
}
