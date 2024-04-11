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

        public void InsertMockLobbyInTestDatebase(string lobbyName, string inviteLink, int lobbyChatId)
        {
            string inserMockLobbyQuery =
                "INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId)" +
                "VALUES (@LobbyName, NULL, @InviteLink, @LobbyChatId)";

            _testDatabaseHelper.RunQuery(inserMockLobbyQuery, new { LobbyName = lobbyName, InviteLink = inviteLink, LobbyChatId = lobbyChatId });
        }

        public void InsertMultipleMockLobbiesInTestDatabase()
        {
            InsertMockLobbyInTestDatebase("LobbyTest1", "linkTest1", 1);
            InsertMockLobbyInTestDatebase("LobbyTest2", "linkTest2", 2);
            InsertMockLobbyInTestDatebase("LobbyTest3", "linkTest3", 3);
            // Add more calls to InsertMockLobbyInTestDatebase as needed
        }

        [Fact]
        public void GetAllGameLobbies_TC1_ReturnsAllGameLobbies()
        {
            InserMockLobbyInTestDatebase();


        }
    }
}
