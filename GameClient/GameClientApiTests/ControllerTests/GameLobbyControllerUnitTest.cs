using GameClientApi.DatabaseAccessors;
using Moq;
using Microsoft.Extensions.Configuration;
using GameClientApi.Controllers;
using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc;


namespace GameClientApiTests.ControllerTests
{
    public class GameLobbyControllerUnitTest{
        
        private IConfiguration _configuration;

        private readonly Mock<IPlayerDatabaseAccessor> _playerMockAccessor;

        private readonly Mock<IGameLobbyDatabaseAccessor> _gameLobbyMockAccessor;


        public GameLobbyControllerUnitTest()
        {
            _playerMockAccessor = new Mock<IPlayerDatabaseAccessor>();

            _gameLobbyMockAccessor = new Mock<IGameLobbyDatabaseAccessor>();
        }
     
        [Fact]
        public void CreateGameLobby_TC1_ReturnsOkwithCreatedGameLobbyWhenNothingIsWrong() 
        {
            //Arrange
            GameLobbyModel mockGameLobby = new GameLobbyModel { LobbyName = "testLobby", AmountOfPlayers = 3, InviteLink = "testLink" };
            _gameLobbyMockAccessor.Setup(a => a.CreateGameLobby(mockGameLobby)).Returns(1);
           
            PlayerModel mockPlayerModel = new PlayerModel { Username = "testPlayer", InGameName = "testPlayer", IsOwner = true };
            _playerMockAccessor.Setup(a => a.GetPlayer("testPlayer")).Returns(mockPlayerModel);

            List<PlayerModel> mockPlayersInLobby = new List<PlayerModel>();
            _playerMockAccessor.Setup(a => a.GetAllPlayersInLobby(1, null)).Returns(mockPlayersInLobby);

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
