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

        public bool VerifyLogin(string userName, string password)
        {

            string selectQueryString = "SELECT * FROM Users WHERE Username = @UserName AND passwordHash = @Password";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var players = connection.Query<Player>(selectQueryString, new {UserName = userName, Password = password});
                return players != null;
            }
        }
    }
}
