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

		private string _connectionString;

		public TestDatabaseHelper(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void TearDownAndBuildTestDatabase()
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.Parent.Parent.FullName;
			string relativePath = Path.Combine(projectDirectory, "SqlScripts", "TearDownAndBuildTablesTestDatabase.sql");
			string fileQueryContents = File.ReadAllText(relativePath);

			// Split the file content into separate commands

			RunTransactionQuery(fileQueryContents);
		}

		public bool RunTransactionQuery(string query)
		{
			bool success = true;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				using (SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						connection.Execute(query, transaction: transaction);
						transaction.Commit();
					}
					catch (SqlException ex)
					{
						success = false;
						transaction.Rollback();
						throw;
					}
				}
			}

			return success;
		}

	}
}
