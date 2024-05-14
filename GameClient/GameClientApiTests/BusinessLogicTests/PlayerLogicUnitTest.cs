using GameClientApi.DatabaseAccessors;
using GameClientApi.BusinessLogic;
using GameClientApi.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GameClientApiTests.BusinessLogicTests
{
	public class PlayerLogicUnitTest
	{

		private readonly IConfiguration? _mockConfiguration;
		private readonly Mock<IPlayerDatabaseAccessor> _mockAccessor;

		public PlayerLogicUnitTest()
		{
			_mockAccessor = new Mock<IPlayerDatabaseAccessor>();
		}

		[Fact]
		public void VerifyLogin_ReturnsTrue_WhenUsernameAndPasswordAreCorrect()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "password";
			string testHashedPassword = "$2a$11$kueqhMMKYY55XvbWELUkjOvGO0BP4f/VjQbMCO27NtLqf8L.smoYm"; // this is 'password' hashed by bcrypt
			PlayerModel testPlayer = new PlayerModel { Banned = false, Username = testUsername, PasswordHash = testHashedPassword };


			_mockAccessor.Setup(a => a.GetPlayer(testUsername, null, null)).Returns(testPlayer);
			
			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.VerifyLogin(testUsername, testPassword);

			// Assert
			Assert.True(testResult, "Should return True but does not");
		}

		[Fact]
		public void VerifyLogin_ThrowsException_WhenUsernameIsInvalid()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "password";
			PlayerModel testPlayer = new PlayerModel { Username = testUsername, PasswordHash = testPassword};

			_mockAccessor.Setup(a => a.GetPlayer("invalidUsername", null, null))
				.Returns(testPlayer);

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

			// Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				// Act
				bool testResult = playerService.VerifyLogin(testUsername, testPassword);
			});
		}

		[Fact]
		public void VerifyLogin_ThrowsException_WhenPasswordForUsernameIsNotFound()
		{
			// Arrange
			string testUsername = "Username";
			string testPassword = "password";

			_mockAccessor.Setup(a => a.GetPlayer("Username", null, null))
				.Returns<string?>(null);

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

			// Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				// Act
				bool testResult = playerService.VerifyLogin(testUsername, testPassword);
			});
		}

		[Fact]
		public void VerifyLogin_ThrowsException_WhenUsernameAndPasswordAreEmpty()
		{
			// Arrange
			string testUsername = "";
			string testPassword = "";

			_mockAccessor.Setup(a => a.GetPlayer(testUsername, null, null))
				.Returns<string?>(null);

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

			// Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				// Act
				bool testResult = playerService.VerifyLogin(testUsername, testPassword);
			});
		}

		[Fact]
		public void VerifyLogin_ThrowsException_WhenUsernameAndPasswordAreNull()
		{
			// Arrange
			string testUsername = null;
			string testPassword = null;

			_mockAccessor.Setup(a => a.GetPlayer(testUsername, null, null))
				.Returns<string?>(null);

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

			// Assert
			Assert.Throws<ArgumentNullException>(() =>
			{
				// Act
				bool testResult = playerService.VerifyLogin(testUsername, testPassword);
			});
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
                BirthDay = DateTime.Now 
            };

			_mockAccessor.Setup(a => a.UsernameExists(mockPlayer.Username))
				.Returns(false);
			_mockAccessor.Setup(a => a.InGameNameExists(mockPlayer.InGameName))
				.Returns(false);
			_mockAccessor.Setup(a => a.CreatePlayer(It.IsAny<AccountRegistrationModel>()))
				.Returns(true);

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

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

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

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

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

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

			PlayerLogic playerService = new PlayerLogic(_mockConfiguration, _mockAccessor.Object);

			// Act
			bool testResult = playerService.CreatePlayer(mockPlayer);

			// Assert
			Assert.False(testResult);
		}

    }
}
