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

		}

		[Fact]
		public void CreatePlayer_TC1_ReturnsOKIfPlayerWasCreated()
		{
			// Arrange
			Player mockPlayer = new Player();

			_mockAccessor.Setup(a => a.UserNameExists(mockPlayer.UserName))
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
			Player mockPlayer = new Player { UserName = "ExistingUsername" };

			_mockAccessor.Setup(a => a.UserNameExists(mockPlayer.UserName))
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
			var modelState = badRequestResult.Value as SerializableError;
			Assert.NotNull(modelState);
			var errorMessages = modelState["UserName"] as string[];
			Assert.Contains("Username already exists.", errorMessages);
		}
		[Fact]
		public void CreatePlayer_TC3_ReturnsBadRequestWhenInGameNameExists()
		{
			// Arrange
			Player mockPlayer = new Player { InGameName = "ExistingInGameName" };

			_mockAccessor.Setup(a => a.UserNameExists(mockPlayer.UserName))
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
			var modelState = badRequestResult.Value as SerializableError;
			Assert.NotNull(modelState);
			var errorMessages = modelState["InGameName"] as string[];
			Assert.Contains("In-game name already exists.", errorMessages);
		}

		[Fact]
		public void CreatePlayer_TC4_ReturnsBadRequestWhenUnknownErrorOccurs()
		{
			// Arrange
			Player mockPlayer = new Player { UserName = "NewUsername", InGameName = "NewInGameName" };

			_mockAccessor.Setup(a => a.UserNameExists(mockPlayer.UserName))
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
			var modelState = badRequestResult.Value as SerializableError;
			Assert.NotNull(modelState);
			var errorMessages = modelState[""] as string[];
			Assert.Contains("Something went wrong when creating the player.", errorMessages);
		}
	}
}
