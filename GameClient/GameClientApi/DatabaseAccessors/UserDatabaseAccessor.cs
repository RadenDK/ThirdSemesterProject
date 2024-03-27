using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
    public class UserDatabaseAccessor
    {

        private readonly string _connectionString;

        public UserDatabaseAccessor(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public IEnumerable<User> GetUsers()
        {

            string selectQueryString = "SELECT * FROM Users";

            IEnumerable<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                users = connection.Query<User>(selectQueryString);

                return users.ToList();
            }

            return users;
        }
    }
}
