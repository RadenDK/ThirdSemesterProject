using Moq;
using Xunit;
using WebClient.Controllers;
using WebClient.Models;
using WebClient.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebClientTest.ControllerTest
{
	public class GameLobbyControllerTest
	{
		[Fact]
		public async Task CreateGameLobby_ReturnsRedirectToActionResult_WhenModelStateIsValid()
		{
			// Arrange
			var mockGameLobbyLogic = new Mock<IGameLobbyLogic>();
			var newLobby = new GameLobbyModel { LobbyName = "TestLobby" };
			var createdLobby = new GameLobbyModel { GameLobbyId = 1, LobbyName = "TestLobby" };
			mockGameLobbyLogic.Setup(x => x.CreateGameLobby(newLobby)).ReturnsAsync(createdLobby);
			var controller = new GameLobbyController(mockGameLobbyLogic.Object);

			// Act
			var result = await controller.CreateGameLobby(newLobby);

			// Assert
			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("GameLobby", redirectToActionResult.ActionName);
			Assert.Equal(createdLobby.GameLobbyId, redirectToActionResult.RouteValues["lobbyId"]);
		}

		[Fact]
		public async Task CreateGameLobby_ReturnsViewResult_WhenModelStateIsInvalid()
		{
			// Arrange
			var mockGameLobbyLogic = new Mock<IGameLobbyLogic>();
			var newLobby = new GameLobbyModel { LobbyName = "TestLobby" };
			var controller = new GameLobbyController(mockGameLobbyLogic.Object);
			controller.ModelState.AddModelError("error", "some error");

			// Act
			var result = await controller.CreateGameLobby(newLobby);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal(newLobby, viewResult.Model);
		}

	}
}
