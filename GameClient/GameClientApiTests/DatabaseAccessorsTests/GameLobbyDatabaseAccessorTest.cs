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

		public void InsertMockPlayersInGameLobbies()
		{
			string query = @"
        INSERT INTO Player (Username, PasswordHash, InGameName, Email, Birthday, Elo, Banned, CurrencyAmount, GameLobbyID, OnlineStatus, IsOwner)
        VALUES ('Player1', 'PasswordHash1', 'InGameName1', 'Email1', '2022-01-01', 1000, 0, 100, 1, 1, 1),
               ('Player2', 'PasswordHash2', 'InGameName2', 'Email2', '2022-01-02', 2000, 0, 200, 2, 1, 0),
               ('Player3', 'PasswordHash3', 'InGameName3', 'Email3', '2022-01-03', 3000, 0, 300, 3, 0, 0);
    ";

			_testDatabaseHelper.RunQuery(query);
		}

		public void InsertMockDataInTestDatabase()
		{
			string query = @"
							SET IDENTITY_INSERT Chat ON;

							INSERT INTO Chat (ChatID, ChatType) VALUES (1, 'Type1');
							INSERT INTO Chat (ChatID, ChatType) VALUES (2, 'Type2');
							INSERT INTO Chat (ChatID, ChatType) VALUES (3, 'Type3');

							SET IDENTITY_INSERT Chat OFF;

							INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId) VALUES ('LobbyNameTest1' , NULL, 'InviteLinkTest1', 1);
							INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId) VALUES ('LobbyNameTest2' , NULL, 'InviteLinkTest2', 2);
							INSERT INTO GameLobby (LobbyName, PasswordHash, InviteLink, LobbyChatId) VALUES ('LobbyNameTest3' , NULL, 'InviteLinkTest3', 3);
						";

			_testDatabaseHelper.RunQuery(query);
		}


		[Fact]
		public void GetAllGameLobbies_TC1_ReturnsAllGameLobbies()
		{
			// Arrange
			InsertMockDataInTestDatabase();
			InsertMockPlayersInGameLobbies();
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
