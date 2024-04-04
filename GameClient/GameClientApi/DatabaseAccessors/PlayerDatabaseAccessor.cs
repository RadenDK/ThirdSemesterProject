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

        public IEnumerable<Player> GetUsers()
        {

            string selectQueryString = "SELECT * FROM Users";

            IEnumerable<Player> users = new List<Player>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                users = connection.Query<Player>(selectQueryString);

                return users.ToList();
            }

            return users;
        }
    }
}
