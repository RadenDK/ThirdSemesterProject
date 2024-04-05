using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
    public class PlayerDatabaseAccessor
    {

        private readonly string _connectionString;

        public PlayerDatabaseAccessor(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public bool GetUserName(string userName)
        {

            string selectQueryString = "SELECT * FROM Users WHERE FirstName = @UserName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var players = connection.Query<Player>(selectQueryString, new {UserName = userName});
                return players != null;
            }
        }

        public bool GetPassword(string password)
        {

            string selectQueryString = "SELECT * FROM Users WHERE LastName = @Password";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var players = connection.Query<Player>(selectQueryString, new { Password = password });
                return players != null;
            }
        }

    }
}
