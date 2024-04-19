using GameClientApi.DatabaseAccessors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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

    }
}
