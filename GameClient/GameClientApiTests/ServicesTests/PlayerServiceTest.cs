using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Services;
using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GameClientApiTests.PlayerServiceTest
{
	public class PlayerServiceTest
	{

		private readonly IConfiguration? _mockConfiguration;
		private readonly Mock<IPlayerDatabaseAccessor> _mockAccessor;

		public PlayerServiceTest()
		{
			_mockAccessor = new Mock<IPlayerDatabaseAccessor>();
		}

		[Fact]
		public void VerifyLogin_TC1_ReturnsTrueWhenInputIsFound()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "ExpectedHashedPassword";

			_mockAccessor.Setup(a => a.GetPassword(testUsername))
				.Returns(testPassword);

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.VerifyLogin(testUsername, testPassword);

			// Assert
			Assert.True(testResult, "Should return True but does not");
		}

		[Fact]
		public void VerifyLogin_TC3_ReturnsFalseWhenUsernameIsNotFound()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "ExpectedHashedPassword";

			_mockAccessor.Setup(a => a.GetPassword("invalidUsername"))
				.Returns("");

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);
		
			// Act
			bool testResult = playerService.VerifyLogin(testUsername, testPassword);

			// Assert
			Assert.False(testResult, "Should return False but does not");
		}

		[Fact]
		public void VerifyLogin_TC4_ReturnsFalseWhenPasswordIsNotFound()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "ExpectedHashedPassword";

			_mockAccessor.Setup(a => a.GetPassword("Username"))
				.Returns("");

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.VerifyLogin(testUsername, testPassword);

			// Assert
			Assert.False(testResult, "Should return False but does not");
		}

		[Fact]
		public void VerifyLogin_TC5_ReturnsFalseWhenUsernameAndPasswordAreEmpty()
		{
			// Arrange
			string testUsername = "";
			string testPassword = "";

			_mockAccessor.Setup(a => a.GetPassword(testUsername))
				.Returns(testPassword);

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.VerifyLogin(testUsername, testPassword);

			// Assert
			Assert.False(testResult, "Should return False but does not");
		}

		[Fact]
		public void VerifyLogin_TC6_ReturnsFalseWhenUsernameAndPasswordAreNull()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "ExpectedHashedPassword";

			_mockAccessor.Setup(a => a.GetPassword(testUsername))
				.Returns("null");

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.VerifyLogin(testUsername, testPassword);

			// Assert
			Assert.False(testResult, "Should return False but does not");
		}
	}
}
