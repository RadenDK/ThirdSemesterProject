using GameClientApi.DatabaseAccessors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GameClientApi.Controllers;
using GameClientApi.BusinessLogic;
using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameClientApiTests.ControllerTests
{
    public class GameLobbyControllerUnitTest{
        
        private IConfiguration _configuration;

        private string _connectionString;

        private readonly Mock<IPlayerDatabaseAccessor> _playerMockAccessor;

        private readonly Mock<IGameLobbyDatabaseAccessor> _gameLobbyMockAccessor;


        public GameLobbyControllerUnitTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _playerMockAccessor = new Mock<IPlayerDatabaseAccessor>();

            _gameLobbyMockAccessor = new Mock<IGameLobbyDatabaseAccessor>();
        }

        [Fact]
        public void AllGameLobbies_TC1_ReturnsOkWithJsonOfGameLobbiesWhenNothingIsWrong()
        {
            //Arrange

            //Act

            //Assert
        }

        [Fact]
        public void AllGameLobbies_TC2_ReturnsBadRequestWhenExceptionOccurs()
        {
            //Arrange

            //Act

            //Assert
        }

        [Fact]
        public void CreateGameLobby_TC1_ReturnsOkwithCreatedGameLobbyWhenNothingIsWrong() 
        {
            //Arrange
            GameLobbyModel mockGameLobby = new GameLobbyModel { LobbyName = "testLobby", AmountOfPlayers = 3, InviteLink = "testLink" };
            _gameLobbyMockAccessor.Setup(a => a.CreateGameLobby(mockGameLobby)).Returns(1);
            PlayerModel mockPlayerModel = new PlayerModel { Username = "testPlayer", InGameName = "testPlayer", IsOwner = true };
            _playerMockAccessor.Setup(a => a.GetPlayer("testPlayer")).Returns(mockPlayerModel);

            GameLobbyController SUT = new GameLobbyController(_configuration, _gameLobbyMockAccessor.Object, _playerMockAccessor.Object);
            CreateGameLobbyModel data = new CreateGameLobbyModel { newLobby = mockGameLobby, username = "testPlayer" };

			//Act
			IActionResult result = SUT.CreateGameLobby(data);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GameLobbyModel>(okResult.Value);
            Assert.Equal(mockGameLobby, returnValue);
        }

        [Fact]
        public void CreateGameLobby_TC2_ReturnsBadRequestWhenExceptionOccurs()
        {
			//Arrange
			GameLobbyModel mockGameLobby = new GameLobbyModel { LobbyName = "testLobby", AmountOfPlayers = 3, InviteLink = "testLink" };
			_gameLobbyMockAccessor.Setup(a => a.CreateGameLobby(mockGameLobby)).Returns(0);
			
			GameLobbyController SUT = new GameLobbyController(_configuration, _gameLobbyMockAccessor.Object, _playerMockAccessor.Object);
			CreateGameLobbyModel data = new CreateGameLobbyModel { newLobby = mockGameLobby, username = "testPlayer" };

			//Act
			IActionResult result = SUT.CreateGameLobby(data);

			//Assert
			Assert.IsType<BadRequestObjectResult>(result);
		}
    }
}
