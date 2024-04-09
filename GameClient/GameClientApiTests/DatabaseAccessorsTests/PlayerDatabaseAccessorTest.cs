using Dapper;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;

using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientApiTests.DatabaseAccessorsTests
{
	public class PlayerDatabaseAccessorTest : IDisposable
	{
		private IConfiguration _configuration;

		private string _connectionString;

		private TestDatabaseHelper _testDatabaseHelper;

		public PlayerDatabaseAccessorTest()
		{
			_configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
		.Build();

			_connectionString = _configuration.GetConnectionString("DefaultConnection");

			_testDatabaseHelper = new TestDatabaseHelper(_connectionString);
		}

		public void Dispose()
		{
			_testDatabaseHelper.TearDownAndBuildTestDatabase();
		}

		public void InsertMockPlayerInTestDatabase()
		{
			string insertMockPlayerQuery =
				"INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) " +
				"VALUES ('Player1', 'hash1', 'InGameName1', GETDATE(), 'player1@example.com');";

			_testDatabaseHelper.RunQuery(insertMockPlayerQuery);
		}

		[Fact]
		public void GetPassword_TC1_ReturnsPasswordOfValidPlayer()
		{
			// Arrange
			InsertMockPlayerInTestDatabase();

			string mockUsername = "Player1";
			string expectedPassword = "hash1";

			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			// Act
			string testResult = SUT.GetPassword(mockUsername);

			// Assert
			Assert.True(expectedPassword == testResult, $"Expected password for {mockUsername} to be {expectedPassword}, but it was {testResult}");
		}

		[Fact]
		public void GetPassword_TC2_ReturnsNullIfUsernameIsNotFound()
		{
			// Arrange
			string mockUsername = "usernameWhichDoesNotExist";

			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			// Act
			string testResult = SUT.GetPassword(mockUsername);

			// Assert
			Assert.True(testResult == null, $"Expected password for {mockUsername} to be null, but it was {testResult}");
		}

		[Fact]
		public void GetPassword_TC3_ReturnsNullIfUsernameIsNull()
		{
			// Arrange
			string? mockUsername = null;

			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			// Act
			string testResult = SUT.GetPassword(mockUsername);

			// Assert
			Assert.True(testResult == null, "Expected password for null username to be null, but it was not");
		}


		[Fact]
		public void InsertPlayer_TC1_InserstANewPlayerAndReturnsSuccessfull()
		{
			// Arrange
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

            AccountRegistrationModel mockPlayer = new AccountRegistrationModel
            {
                Username = "username1",
                Password = "password1",
                Email = "email1@example.com",
                InGameName = "InGameName1",
                BirthDay = DateTime.Now // or any other DateTime value
            };


            // Act
            bool testResult = SUT.CreatePlayer(mockPlayer);

			// Assert
			Assert.True(testResult);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM Player WHERE Username = @Username";
				IEnumerable<string> queryResult = connection.Query<string>(query, new { UserName = mockPlayer.Username });
				Assert.True(queryResult.Any(), "Expected a mock player to be inserted in the database but could not find it");
			}
		}

		[Fact]
		public void InsertPlayer_TC2_MethodDoesNotInsertPlayerWithMissingInformation()
		{
			// Arrange
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			AccountRegistrationModel mockPlayer = new AccountRegistrationModel { Username = "username1" };

			// Act
			bool testResult = SUT.CreatePlayer(mockPlayer);

			// Assert
			Assert.False(testResult);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM Player WHERE Username = @Username";
				IEnumerable<string> queryResult = connection.Query<string>(query, new { UserName = mockPlayer.Username });
				Assert.True(!queryResult.Any(), "Expected not to find mock player in the database but found one");
			}
		}

		[Fact]
		public void InsertPlayer_TC2_MethodDoesNotInsertPlayerWhenPlayerIsNull()
		{
			// Arrange
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			AccountRegistrationModel mockPlayer = null;

			// Act
			bool testResult = SUT.CreatePlayer(mockPlayer);

			// Assert
			Assert.False(testResult);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM Player WHERE Username = @Username";
				IEnumerable<string> queryResult = connection.Query<string>(query, new { UserName = mockPlayer.Username });
				Assert.True(!queryResult.Any(), "Expected not to find mock player in the database but found one");
			}
		}

		public void UserNameExists_TC1_ReturnsTrueWhenUsernameExists()
		{
			// Arrange
			InsertMockPlayerInTestDatabase();
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			string mockUsername = "Username";

			// Act
			bool testResult = SUT.UserNameExists(mockUsername);

			// Assert
			Assert.True(testResult);
		}

		public void UserNameExists_TC2_ReturnsFalseWhenUsernameIsNull()
		{
			// Arrange
			InsertMockPlayerInTestDatabase();
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			string mockUsername = null;

			// Act
			bool testResult = SUT.UserNameExists(mockUsername);

			// Assert
			Assert.False(testResult);
		}

		public void UserNameExists_TC3_ReturnsFalseWhenUsernameDoesNotExist()
		{
			// Arrange
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			string mockUsername = "Username";

			// Act
			bool testResult = SUT.UserNameExists(mockUsername);

			// Assert
			Assert.False(testResult);
		}
		[Fact]
		public void InGameNameExists_TC1_ReturnsTrueWhenInGameNameExists()
		{
			// Arrange
			InsertMockPlayerInTestDatabase();
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			string mockInGameName = "InGameName1";

			// Act
			bool testResult = SUT.InGameNameExists(mockInGameName);

			// Assert
			Assert.True(testResult);
		}

		[Fact]
		public void InGameNameExists_TC2_ReturnsFalseWhenInGameNameIsNull()
		{
			// Arrange
			InsertMockPlayerInTestDatabase();
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			string mockInGameName = null;

			// Act
			bool testResult = SUT.InGameNameExists(mockInGameName);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void InGameNameExists_TC3_ReturnsFalseWhenInGameNameDoesNotExist()
		{
			// Arrange
			PlayerDatabaseAccessor SUT = new PlayerDatabaseAccessor(_configuration);

			string mockInGameName = "NonExistentInGameName";

			// Act
			bool testResult = SUT.InGameNameExists(mockInGameName);

			// Assert
			Assert.False(testResult);
		}
	}
}
