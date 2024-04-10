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

namespace GameClientApiTests.PlayerControllerTests
{
    public class PlayerControllerTest
    {

        private IConfiguration _configuration;

        private string _connectionString;

        private readonly Mock<IPlayerDatabaseAccessor> _mockAccessor;


        public PlayerControllerTest()
        {
            _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
        .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _mockAccessor = new Mock<IPlayerDatabaseAccessor>();
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
            _mockAccessor.Setup(a => a.CreatePlayer(mockPlayer))
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
    }
}
