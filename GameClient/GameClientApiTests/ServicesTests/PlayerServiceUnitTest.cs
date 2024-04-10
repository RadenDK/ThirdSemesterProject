using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Services;
using GameClientApi.Models;
using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GameClientApiTests.PlayerServiceTest
{
	public class PlayerServiceUnitTest
	{

		private readonly IConfiguration? _mockConfiguration;
		private readonly Mock<IPlayerDatabaseAccessor> _mockAccessor;

		public PlayerServiceUnitTest()
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

		[Fact]
		public void CreatePlayer_TC1_ReturnsTrueIfPlayerIsValid()
		{
            // Arrange
            AccountRegistrationModel mockPlayer = new AccountRegistrationModel
            {
                Username = "username1",
                Password = "password1",
                Email = "email1@example.com",
                InGameName = "InGameName1",
                BirthDay = DateTime.Now // or any other DateTime value
            };

			_mockAccessor.Setup(a => a.UsernameExists(mockPlayer.Username))
				.Returns(false);
			_mockAccessor.Setup(a => a.InGameNameExists(mockPlayer.InGameName))
				.Returns(false);
			_mockAccessor.Setup(a => a.CreatePlayer(mockPlayer))
				.Returns(true);

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.CreatePlayer(mockPlayer);

			// Assert
			Assert.True(testResult);
		}

		[Fact]
		public void CreatePlayer_TC2_ThrowsExpectionIfUsernameDoesNotExist()
		{
            // Arrange
            AccountRegistrationModel mockPlayer = new AccountRegistrationModel
            {
                Username = "username1",
                Password = "password1",
                Email = "email1@example.com",
                InGameName = "InGameName1",
                BirthDay = DateTime.Now // or any other DateTime value
            };

            _mockAccessor.Setup(a => a.UsernameExists(mockPlayer.Username))
				.Returns(true);
			_mockAccessor.Setup(a => a.InGameNameExists(mockPlayer.InGameName))
				.Returns(false);
			_mockAccessor.Setup(a => a.CreatePlayer(mockPlayer))
				.Returns(false);

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Assert
			Assert.Throws<ArgumentException>(() =>
			{
				// Act
				bool testResult = playerService.CreatePlayer(mockPlayer);

			});
		}

		[Fact]
		public void CreatePlayer_TC3_ThrowsExpectionIfIngamenameDoesNotExist()
		{
            // Arrange
            AccountRegistrationModel mockPlayer = new AccountRegistrationModel
            {
                Username = "username1",
                Password = "password1",
                Email = "email1@example.com",
                InGameName = "InGameName1",
                BirthDay = DateTime.Now // or any other DateTime value
            };

            _mockAccessor.Setup(a => a.UsernameExists(mockPlayer.Username))
				.Returns(false);
			_mockAccessor.Setup(a => a.InGameNameExists(mockPlayer.InGameName))
				.Returns(true);
			_mockAccessor.Setup(a => a.CreatePlayer(mockPlayer))
				.Returns(false);

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Assert
			Assert.Throws<ArgumentException>(() =>
			{
				// Act
				bool testResult = playerService.CreatePlayer(mockPlayer);

			});
		}

		[Fact]
		public void CreatePlayer_TC4_ReturnsFalsePlayerWasNotCreated()
		{
            // Arrange
            AccountRegistrationModel mockPlayer = new AccountRegistrationModel
            {
                Username = "username1",
                Password = "password1",
                Email = "email1@example.com",
                InGameName = "InGameName1",
                BirthDay = DateTime.Now // or any other DateTime value
            };

            _mockAccessor.Setup(a => a.UsernameExists(mockPlayer.Username))
				.Returns(false);
			_mockAccessor.Setup(a => a.InGameNameExists(mockPlayer.InGameName))
				.Returns(false);
			_mockAccessor.Setup(a => a.CreatePlayer(mockPlayer))
				.Returns(false);

			PlayerService playerService = new PlayerService(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.CreatePlayer(mockPlayer);

			// Assert
			Assert.False(testResult);
		}

    }
}
