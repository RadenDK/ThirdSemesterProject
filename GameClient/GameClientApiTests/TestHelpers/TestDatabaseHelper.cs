using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientApiTests.TestHelpers
{
	internal class TestDatabaseHelper
	{

		private IConfiguration _configuration;
		private string _connectionString;

		public TestDatabaseHelper(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("TestDatabase");
		}


		public void TearDownAndBuildTestDatabase()
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string relativePath = Path.Combine(currentDirectory, "..", "..", "..", "..", "SqlScripts", "TearDownAndBuildTablesTestDatabase.sql");
			string absolutePath = Path.GetFullPath(relativePath);

			string fileQueryContents = File.ReadAllText(absolutePath);

			// Split the file content into separate commands

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				connection.Execute(fileQueryContents);
			}
		}

		public bool RunQuery(string query)
		{
			bool success = true;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				try
				{
					connection.Execute(query);
				}
				catch (SqlException ex)
				{
					success = false;
					throw;
				}
			}

			return success;
		}
	}
}
