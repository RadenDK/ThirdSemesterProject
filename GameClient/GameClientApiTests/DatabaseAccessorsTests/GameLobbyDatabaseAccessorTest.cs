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
	[Collection("Sequential")]
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

			_testDatabaseHelper.RunTransactionQuery(query);
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
			List<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.NotNull(testResult);
			Assert.False(testResult.Any());
		}

		[Fact]
		public void CreateGameLobby_TC1_InsertsANewGameLobbyAndReturnsSuccessful()
		{
			// Arrange
			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

			LobbyChatModel mockLobbyChat = new LobbyChatModel { ChatId = 1, ChatType = "Type1" };
			GameLobbyModel mockGameLobby = new GameLobbyModel
			{
				LobbyName = "TestLobby",
				PasswordHash = "passwordHash",
				InviteLink = "inviteLinkTest",
				LobbyChat = mockLobbyChat
			};

			// Act
			bool testResult = SUT.CreateGameLobby(mockGameLobby);

			// Assert
			Assert.True(testResult);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM GameLobby WHERE LobbyName = @LobbyName";
				IEnumerable<string> queryResult = connection.Query<string>(query, new { LobbyName = mockGameLobby.LobbyName });
				Assert.True(queryResult.Any(), "Expected a mock game lobby to be inserted in the database but could not find it");
			}
		}

		[Fact]
		public void CreateGameLobby_TC2_MethodDoesNotInsertGameLobbyWithMissingInformation()
		{
			// Arrange
			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

			GameLobbyModel mockGameLobby = new GameLobbyModel { LobbyName = "TestLobby" };

			// Act
			bool testResult = SUT.CreateGameLobby(mockGameLobby);

			// Assert
			Assert.False(testResult);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM GameLobby WHERE LobbyName = @LobbyName";
				IEnumerable<string> queryResult = connection.Query<string>(query, new { LobbyName = mockGameLobby.LobbyName });
				Assert.False(queryResult.Any(), "Expected not to find mock game lobby in the database but found one");
			}
		}

		[Fact]
		public void CreateGameLobby_TC3_MethodDoesNotInsertGameLobbyWhenGameLobbyIsNull()
		{
			// Arrange
			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

			GameLobbyModel mockGameLobby = null;

			// Act
			bool testResult = SUT.CreateGameLobby(mockGameLobby);

			// Assert
			Assert.False(testResult);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM GameLobby";
				IEnumerable<string> queryResult = connection.Query<string>(query);
				Assert.False(queryResult.Any(), "Expected not to find mock game lobby in the database but found one");
			}
		}


	}
}
