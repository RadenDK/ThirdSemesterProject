using Dapper;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;

using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClientApiTests.DatabaseAccessorsTests
{
    public class GameLobbyDatabaseAccessorTest : IDisposable
    {
        private IConfiguration _configuration;

        private string _connectionString;

        private TestDatabaseHelper _testDatabaseHelper;

        public GameLobbyDatabaseAccessorTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsForTesting.json", optional: false, reloadOnChange: true)
                .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            _testDatabaseHelper = new TestDatabaseHelper(_connectionString);

            _testDatabaseHelper.TearDownAndBuildTestDatabase();
        }

        public void Dispose()
        {
            _testDatabaseHelper.TearDownAndBuildTestDatabase();
        }

        public void InsertMockLobbyInTestDatebase()
        {
            string insertMockLobbyQuery1 =
                "INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId)" +
                "VALUES ('LobbyNameTest1' , NULL, 'InviteLinkTest1', 1)";
            string insertMockLobbyQuery2 =
                "INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId)" +
                "VALUES ('LobbyNameTest2' , NULL, 'InviteLinkTest2', 2)";
            string insertMockLobbyQuery3 =
                "INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId)" +
                "VALUES ('LobbyNameTest3' , NULL, 'InviteLinkTest3', 3)";

            _testDatabaseHelper.RunQuery(insertMockLobbyQuery1);
            _testDatabaseHelper.RunQuery(insertMockLobbyQuery2);
            _testDatabaseHelper.RunQuery(insertMockLobbyQuery3);
        }

        [Fact]
        public void GetAllGameLobbies_TC1_ReturnsAllGameLobbies()
        {
            // Arrange
            InsertMockLobbyInTestDatebase();
            GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

            // Act
            List<GameLobbyModel> result = SUT.GetAllGameLobbies();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count); // Replace 3 with the number of lobbies you inserted
                                           // Add more assertions here to verify that the returned lobbies are correct
   
        }
    }
}
