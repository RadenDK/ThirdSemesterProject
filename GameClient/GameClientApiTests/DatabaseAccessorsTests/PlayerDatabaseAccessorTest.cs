using GameClientApi.DatabaseAccessors;
using GameClientApiTests.TestHelpers;
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

		private TestDatabaseHelper _testDatabaseHelper;

		public PlayerDatabaseAccessorTest()
		{
			_configuration = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
		.Build();

			_testDatabaseHelper = new TestDatabaseHelper(_configuration.GetConnectionString("DefaultConnection"));
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
	}
}
