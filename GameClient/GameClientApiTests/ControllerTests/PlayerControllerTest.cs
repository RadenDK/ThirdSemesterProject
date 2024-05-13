using Microsoft.Extensions.Configuration;
using GameClientApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClientApi.DatabaseAccessors;
using Moq;
using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GameClientApiTests.TestHelpers;
using GameClientApi.BusinessLogic;
using Microsoft.OpenApi.Any;
using Microsoft.Data.SqlClient;

namespace GameClientApiTests.PlayerControllerTests
{
	[Collection("Sequential")]
	public class PlayerControllerTest : IDisposable
    {

        private IConfiguration _configuration;

        private string _connectionString;

        private readonly Mock<IPlayerDatabaseAccessor> _mockAccessor;

        private TestDatabaseHelper _testDatabaseHelper;


        public PlayerControllerTest()
        {
            _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
        .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _mockAccessor = new Mock<IPlayerDatabaseAccessor>();

            _testDatabaseHelper = new TestDatabaseHelper(_connectionString);
        }

		public void Dispose()
		{
            _testDatabaseHelper.TearDownAndBuildTestDatabase();
		}

		private void InsertMockPlayerInTestDatabase()
        {
            string insertMockPlayerQuery =
                "INSERT INTO Player (Username, PasswordHash, InGameName, Birthday, Email) " +
                "VALUES ('Player1', '$2a$11$GsmfIz3OPipR6f5avJUDTuFMItDbPZtiCmYScex0uZxo1z4Q6iP/i', 'InGameName1', GETDATE(), 'player1@example.com');";

            _testDatabaseHelper.RunTransactionQuery(insertMockPlayerQuery);
        }

        [Fact]
        public void CreatePlayer_TC1_ReturnsOKIfPlayerWasCreated()
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
            _mockAccessor.Setup(a => a.CreatePlayer(It.IsAny<AccountRegistrationModel>()))
                .Returns(true);

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.CreatePlayer(mockPlayer);

            // Assert
            Assert.IsType<OkResult>(testResult);
        }

        [Fact]
        public void CreatePlayer_TC2_ReturnsBadRequestWhenUsernameExists()
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

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.CreatePlayer(mockPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(testResult);
            var serializedValue = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserializedValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedValue);
            Assert.NotNull(deserializedValue);
            Assert.True(deserializedValue.ContainsKey("message"));
            Assert.Equal("Username already exists", deserializedValue["message"]);
        }

        [Fact]
        public void CreatePlayer_TC3_ReturnsBadRequestWhenInGameNameExists()
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

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.CreatePlayer(mockPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(testResult);
            var serializedValue = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserializedValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedValue);
            Assert.NotNull(deserializedValue);
            Assert.True(deserializedValue.ContainsKey("message"));
            Assert.Equal("InGameName already exists", deserializedValue["message"]);
        }


        [Fact]
        public void CreatePlayer_TC4_ReturnsBadRequestWhenUnknownErrorOccurs()
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

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.CreatePlayer(mockPlayer);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(testResult);
            var serializedValue = JsonConvert.SerializeObject(badRequestResult.Value);
            var deserializedValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedValue);
            Assert.NotNull(deserializedValue);
            Assert.True(deserializedValue.ContainsKey("message"));
            Assert.Equal("Error creating the player", deserializedValue["message"]);
        }

        [Fact]
        public void DoesPlayerExist_TC1_ReturnsOkWhenPlayerExists()
        {
			// Arrange
			InsertMockPlayerInTestDatabase();

			LoginModel mockLogin = new LoginModel
            {
                Username = "Player1",
                Password = "Player1"
            };

            PlayerModel expectedPlayer = new PlayerModel { Username = mockLogin.Username };

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(mockLogin.Password);

            _mockAccessor.Setup(a => a.GetPassword(mockLogin.Username)).Returns(hashedPassword);
			_mockAccessor.Setup(a => a.GetPlayer(mockLogin.Username)).Returns(expectedPlayer);

			// Insert a mock player into the test database

			PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.DoesPlayerExist(mockLogin);

            // Assert
            Assert.IsType<OkObjectResult>(testResult);
        }

        [Fact]
        public void DoesPlayerExist_TC2_ReturnsBadRequestWhenPlayerDoesNotExist()
        {
            // Arrange
            LoginModel mockLogin = new LoginModel
            {
                Username = "nonExistingUser",
                Password = "wrongPassword"
            };

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.DoesPlayerExist(mockLogin);

            // Assert
            Assert.IsType<BadRequestObjectResult>(testResult);
        }

        [Fact]
        public void BanPlayer_TC1_ReturnsOKWhenPlayerIsBanned()
        {
            // Arrange
            int playerIdToBan = 1;
            PlayerModel playerToBan = new PlayerModel { PlayerId = playerIdToBan, Banned = false };

            _mockAccessor.Setup(a => a.GetPlayer(It.IsAny<string>())).Returns(playerToBan);
            _mockAccessor.Setup(a => a.BanPlayer(It.IsAny<PlayerModel>(), It.IsAny<SqlTransaction>())).Returns(true);

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.BanPlayer(playerIdToBan.ToString());

            // Assert
            Assert.IsType<OkResult>(testResult);
        }

        [Fact]
        public void BanPlayer_TC2_ReturnsBadRequestWhenPlayerDoesNotExist()
        {
            // Arrange
            string nonExistingPlayerId = "nonExistingUser";

            _mockAccessor.Setup(a => a.GetPlayer(It.IsAny<string>())).Returns((PlayerModel)null);

            PlayerController SUT = new PlayerController(_configuration, _mockAccessor.Object);

            // Act
            IActionResult testResult = SUT.BanPlayer(nonExistingPlayerId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(testResult);
        }
    }
}
