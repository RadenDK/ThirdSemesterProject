using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GameClientApi.Services;
using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GameClientApiTests.PlayerServiceTest
{
	public class PlayerServiceTest : IDisposable
	{

		private IConfiguration _configuration;
		private string _connectionString;

		private TestDatabaseHelper _testDatabaseHelper;

		public PlayerServiceTest(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("TestDatabase");
			_testDatabaseHelper = new TestDatabaseHelper(configuration);
		}

		public void Dispose()
		{
			_testDatabaseHelper.TearDownAndBuildTestDatabase();
		}

		private void InsertValidPlayerMockData()
		{
			string query = "";
			_testDatabaseHelper.RunQuery(query);
		}

		private void InsertInvalidPlayerMockData() {
			string query = "";
			_testDatabaseHelper.RunQuery(query);
		}

		[Fact]
		public void VerifyLogin_TC1_ReturnsTrueWhenInputIsFound()
		{
			// Arrange
			InsertValidPlayerMockData();
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "validUsername";
			string mockPassword = "validHashedPassword";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.True(testResult);
		}

		[Fact]
		public void VerifyLogin_TC2_ReturnsFalseWhenInputIsNotFound()
		{
			// Arrange
			InsertInvalidPlayerMockData();
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "invalidlUsername";
			string mockPassword = "invalidHashedPassword";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void VerifyLogin_TC3_ReturnsFalseWhenUsernameIsNotFound()
		{
			// Arrange
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "invalidlUsername";
			string mockPassword = "validHashedPassword";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void VerifyLogin_TC4_ReturnsFalseWhenPasswordIsNotFound()
		{
			// Arrange
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "validlUsername";
			string mockPassword = "invalidHashedPassword";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void VerifyLogin_TC5_ReturnsFalseWhenUsernameAndPasswordAreEmpty()
		{
			// Arrange
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "";
			string mockPassword = "";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void VerifyLogin_TC6_ReturnsFalseWhenUsernameAndPasswordAreNull()
		{
			// Arrange
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = null;
			string mockPassword = null;

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void VerifyLogin_TC7_ReturnsFalseWhenPasswordIsEmpty()
		{
			// Arrange
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "validUsername";
			string mockPassword = "";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

		[Fact]
		public void VerifyLogin_TC8_ReturnsFalseWhenUsernameIsEmpty()
		{
			// Arrange
			PlayerService playerService = new PlayerService(_configuration);
			string mockUsername = "";
			string mockPassword = "validHashedPassword";

			// Act
			bool testResult = playerService.VerifyLogin(mockUsername, mockPassword);

			// Assert
			Assert.False(testResult);
		}

	}
}
