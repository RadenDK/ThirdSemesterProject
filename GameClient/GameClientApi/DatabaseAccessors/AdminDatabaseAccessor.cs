using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace GameClientApi.DatabaseAccessors
{
    public class AdminDatabaseAccessor : IAdminDatabaseAccessor
    {
        private readonly string _connectionString;

        public AdminDatabaseAccessor(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string? GetPassword(int adminId)
        {
            string selectQueryString = "SELECT PasswordHash FROM Admin WHERE AdminId = @AdminId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var password = connection.QuerySingleOrDefault<string>(selectQueryString, new { AdminId = adminId });
                return password;
            }
        }

        public AdminModel GetAdmin(int adminId)
        {
            string selectQueryString = "SELECT * FROM Admin WHERE AdminId = @AdminId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var admin = connection.QuerySingleOrDefault<AdminModel>(selectQueryString, new { AdminId = adminId });
                return admin;
            }
        }
    }
}