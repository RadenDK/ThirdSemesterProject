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
            bool playerInserted = false;

            if (AccountHasValues(newPlayer))
            {
                string insertQuery = "INSERT INTO Player (Username, PasswordHash, InGameName, Email, Birthday) " +
                    "VALUES (@Username, @Password, @InGameName, @Email, @Birthday)";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    var rowsAffected = connection.Execute(insertQuery, newPlayer);
                    playerInserted = rowsAffected == 1;
                }
            }

            return playerInserted;
        }

        private bool AccountHasValues(AccountRegistrationModel newAccount)
        {
            if (newAccount == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(newAccount.Username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newAccount.Password))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newAccount.Email))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newAccount.InGameName))
            {
                return false;
            }
            if (newAccount.BirthDay == null || newAccount.BirthDay < new DateTime(1753, 1, 1) || newAccount.BirthDay > new DateTime(9999, 12, 31))
            {
                return false;
            }

            return true;
        }

        public bool UsernameExists(string username)
        {
            string selectQueryString = "SELECT 1 FROM Player WHERE Username = @UserName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var player = connection.QuerySingleOrDefault<string>(selectQueryString, new { UserName = username });
                return player != null;
            }
        }

        public bool InGameNameExists(string inGameName)
        {
            string selectQueryString = "SELECT 1 FROM Player WHERE InGameName = @inGameName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var player = connection.QuerySingleOrDefault<string>(selectQueryString, new { InGameName = inGameName });
                return player != null;
            }
        }

    }
}
