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

		public void InsertMockGameLobbiesWithChatsInTestDatabase()
		{
			string query = @"
					SET IDENTITY_INSERT Chat ON;

					INSERT INTO Chat (ChatID, ChatType) VALUES (1, 'Type1');
					INSERT INTO Chat (ChatID, ChatType) VALUES (2, 'Type2');
					INSERT INTO Chat (ChatID, ChatType) VALUES (3, 'Type3');

					SET IDENTITY_INSERT Chat OFF;

					SET IDENTITY_INSERT GameLobby ON;

					INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, InviteLink, LobbyChatId) VALUES (1, 'LobbyNameTest1' , NULL, 'InviteLinkTest1', 1);
					INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, InviteLink, LobbyChatId) VALUES (2, 'LobbyNameTest2' , NULL, 'InviteLinkTest2', 2);
					INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, InviteLink, LobbyChatId) VALUES (3, 'LobbyNameTest3' , NULL, 'InviteLinkTest3', 3);
							
					SET IDENTITY_INSERT GameLobby OFF;";

			_testDatabaseHelper.RunQuery(query);
		}


		[Fact]
		public void GetAllGameLobbies_TC1_ReturnsAllGameLobbies()
		{
			// Arrange
			InsertMockGameLobbiesWithChatsInTestDatabase();

			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

			// Act
			List<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.NotNull(testResult);
			Assert.Equal(3, testResult.Count);
			Assert.Equal("LobbyNameTest1", testResult[0].LobbyName);

			Assert.All(testResult, lobby =>
			{
				Assert.NotNull(lobby.LobbyName);
				Assert.NotNull(lobby.InviteLink);
			});
		}

		[Fact]
		public void GetAlleGameLobbies_TC2_ReturnEmptyListWhenNoGameLobbiesAreInDatabase()
		{
			// Arrange 
			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

			// Act
			List<GameLobbyModel> testResult = SUT.GetAllGameLobbies().ToList();

			// Assert
			Assert.NotNull(testResult);
			Assert.False(testResult.Any());
		}
	}
}
