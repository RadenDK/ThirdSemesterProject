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

			LobbyChatModel mockLobbyChat = new LobbyChatModel { LobbyChatId = 1, ChatType = "Type1" };
			GameLobbyModel mockGameLobby = new GameLobbyModel
			{
				LobbyName = "LobbyNameTest1",
				PasswordHash = "passwordHash1",
				InviteLink = "inviteLinkTest1",
				LobbyChat = mockLobbyChat,
				AmountOfPlayers = 3
			};

			// Act
			int testResult = SUT.CreateGameLobby(mockGameLobby);

			// Assert
			Assert.Equal(testResult, 1);
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
			int testResult = SUT.CreateGameLobby(mockGameLobby);

			// Assert
			Assert.Equal(testResult, 0);
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
			int testResult = SUT.CreateGameLobby(mockGameLobby);

			// Assert
			Assert.Equal(testResult, 0);
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				string query = "SELECT 1 FROM GameLobby";
				IEnumerable<string> queryResult = connection.Query<string>(query);
				Assert.False(queryResult.Any(), "Expected not to find mock game lobby in the database but found one");
			}
		}

		[Fact]
		public void GetGameLobby_TC1_MethodReturnsAValidLobbyInDatabase()
		{
			// Arrange
			InsertMockGameLobbiesWithChatsInTestDatabase(); 

			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);


			// Act
			GameLobbyModel testResult = SUT.GetGameLobby(1);

			// Assert
			Assert.True(testResult != null, "Expected a GameLobbyModel, but got null");
			Assert.True(testResult.GameLobbyId == 1, $"Expected GameLobbyId to be 1, but got {testResult.GameLobbyId}");
			Assert.True(testResult.LobbyName == "LobbyNameTest1", $"Expected LobbyName to be 'LobbyNameTest1', but got {testResult.LobbyName}");
			Assert.True(testResult.PasswordHash == null, $"Expected PasswordHash to be null, but got {testResult.PasswordHash}");
			Assert.True(testResult.InviteLink == "InviteLinkTest1", $"Expected InviteLink to be 'InviteLinkTest1', but got {testResult.InviteLink}");
			Assert.True(testResult.LobbyChat != null, "Expected a LobbyChatModel, but got null");
			Assert.True(testResult.LobbyChat.LobbyChatId == 1, $"Expected LobbyChat.ChatId to be 1, but got {testResult.LobbyChat.LobbyChatId}");
		}

		[Fact]
		public void GetGameLobby_TC2_MethodThrowsExpectionIfLobbyCouldNotBeFound()
		{
			// Arrange
			GameLobbyDatabaseAccessor SUT = new GameLobbyDatabaseAccessor(_configuration);

			// Assert

			Assert.Throws<Exception>(() =>
			{
				// Act

				GameLobbyModel testResult = SUT.GetGameLobby(1);
			});
		}

	}
}
