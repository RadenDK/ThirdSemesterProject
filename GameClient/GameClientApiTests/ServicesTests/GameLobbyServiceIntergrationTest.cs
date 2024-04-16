using GameClientApi.DatabaseAccessors;
using GameClientApi.Services;
using GameClientApiTests.TestHelpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using Azure.Identity;



namespace GameClientApiTests.ServicesTests
{
	[Collection("Sequential")]
	public class GameLobbyServiceIntergrationTest : IDisposable
	{
		private IConfiguration _configuration;

		private string _connectionString;

		private TestDatabaseHelper _testDatabaseHelper;

		public GameLobbyServiceIntergrationTest()
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

		private void InsertMockGameLobbies()
		{
			string query = @"
			SET IDENTITY_INSERT GameLobby ON;

			INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, AmountOfPlayer, InviteLink, LobbyChatId) VALUES (1, 'LobbyNameTest1' , NULL, 10, 'InviteLinkTest1', 1);
			INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, AmountOfPlayer, InviteLink, LobbyChatId) VALUES (2, 'LobbyNameTest2' , NULL, 10, 'InviteLinkTest2', 2);
			INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, AmountOfPlayer, InviteLink, LobbyChatId) VALUES (3, 'LobbyNameTest3' , NULL, 10, 'InviteLinkTest3', 3);
							
			SET IDENTITY_INSERT GameLobby OFF;";

			_testDatabaseHelper.RunTransactionQuery(query);
		}

		private void InsertMockPlayersInGameLobbies()
		{
			string query = @"
			SET IDENTITY_INSERT Player ON;

        INSERT INTO Player (PlayerId, Username, PasswordHash, InGameName, Birthday)
        VALUES (1, 'Player1', 'PasswordHash1', 'InGameName1', '2022-01-01'),
               (2, 'Player2', 'PasswordHash2', 'InGameName2', '2022-01-02'),
               (3, 'Player3', 'PasswordHash3', 'InGameName3', '2022-01-03'),
               (4, 'Player4', 'PasswordHash4', 'InGameName4', '2022-01-04'),
               (5, 'Player5', 'PasswordHash5', 'InGameName5', '2022-01-05'),
               (6, 'Player6', 'PasswordHash6', 'InGameName6', '2022-01-06');";

			_testDatabaseHelper.RunTransactionQuery(query);
		}

		private void AssosiatePlayerWithGameLobbies()
		{
			string query = @"
        UPDATE Player SET GameLobbyId = 1 WHERE PlayerId IN (1);
        UPDATE Player SET GameLobbyId = 2 WHERE PlayerId IN (2, 3);
        UPDATE Player SET GameLobbyId = 3 WHERE PlayerId IN (4,5,6);";

			_testDatabaseHelper.RunTransactionQuery(query);
		}


		private void SetValidOwnershipOfGameLobbies()
		{
			string query = @"
			UPDATE Player
			SET IsOwner = 1
			WHERE PlayerId IN (1, 2, 4)";

			_testDatabaseHelper.RunTransactionQuery(query);
		}

		private void SetInvalidOwnershipOfGameLobbie()
		{
			string query = @"
			UPDATE Player
			SET IsOwner = 1
			WHERE PlayerId IN (1, 2, 3, 4, 5, 6)";

			_testDatabaseHelper.RunTransactionQuery(query);
		}


		[Fact]
		public void GetAllGameLobbies_TC1_ReturnsExpectedGameLobbies()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiatePlayerWithGameLobbies();
			SetValidOwnershipOfGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerService playerService = new PlayerService(_configuration, playerDatabaseAccessor);
			GameLobbyService SUT = new GameLobbyService(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.True(testResult != null, "Test result is null.");
			Assert.True(testResult.Count() == 2, "Test result count is not 2.");

			foreach (var gameLobby in testResult)
			{
				Assert.True(gameLobby.GameLobbyId != 0, "Game lobby ID is 0.");
				Assert.True(gameLobby.LobbyName != null, "Game lobby name is null.");
				Assert.True(gameLobby.InviteLink != null, "Game lobby invite link is null.");
				Assert.True(gameLobby.PlayersInLobby != null, "Players in game lobby is null.");
				Assert.True(gameLobby.PlayersInLobby.Count(player => player.IsOwner) == 1, "There is not exactly one owner in the game lobby.");
				Assert.True(gameLobby.PlayersInLobby.All(player => !string.IsNullOrEmpty(player.InGameName)), "One or more players in the game lobby do not have an InGameName.");
				Assert.True(gameLobby.PlayersInLobby.Count <= gameLobby.AmountOfPlayers, "The number of players in the game lobby exceeds the amount of players.");
			}
		}

		[Fact]
		public void GetAllGameLobbies_TC2_ReturnsLobbiesWithAtLeastOnePlayer()
		{
			// Arrange
			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerService playerService = new PlayerService(_configuration, playerDatabaseAccessor);
			GameLobbyService SUT = new GameLobbyService(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();


			// Assert
			// This test will check that the GetAllGameLobbies method does not return any lobbies that have no players.
		}

		[Fact]
		public void GetAllGameLobbies_TC3_ReturnsLobbiesWithOnlyOneOwner()
		{
			// Arrange

			// Act

			// Assert
			// This test will check that the GetAllGameLobbies method only returns lobbies that have exactly one owner.
		}

		[Fact]
		public void GetAllGameLobbies_TC4_DoesNotReturnLobbiesWithTooManyOwners()
		{
			// Arrange

			// Act

			// Assert
			// This test will check that the GetAllGameLobbies method does not return any lobbies that have more than one owner.
		}

		[Fact]
		public void GetAllGameLobbies_TC5_DoesNotReturnLobbiesWithNoPlayers()
		{
			// Arrange

			// Act

			// Assert
			// This test will check that the GetAllGameLobbies method does not return any lobbies that have no players.
		}

		[Fact]
		public void GetAllGameLobbies_TC6_DoesNotReturnLobbiesWithTooManyPlayers()
		{
			// Arrange

			// Act

			// Assert
			// This test will check that the GetAllGameLobbies method does not return any lobbies that have more players than the specified capacity.
		}


	}
}
