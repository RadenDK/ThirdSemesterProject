using GameClientApi.DatabaseAccessors;
using GameClientApi.BusinessLogic;
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

			SET IDENTITY_INSERT Chat ON;

			INSERT INTO Chat (ChatId, ChatType) VALUES (1, 'LobbyChat'), (2, 'LobbyChat'), (3, 'LobbyChat')

			SET IDENTITY_INSERT Chat OFF;

			SET IDENTITY_INSERT GameLobby ON;

			INSERT INTO GameLobby (GameLobbyId, LobbyName, PasswordHash, AmountOfPlayers, InviteLink, LobbyChatId) VALUES 
			(1, 'LobbyNameTest1' , NULL, 1, 'InviteLinkTest1', 1),
			(2, 'LobbyNameTest2' , NULL, 2, 'InviteLinkTest2', 2),
			(3, 'LobbyNameTest3' , NULL, 3, 'InviteLinkTest3', 3);
							
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

		private void AssosiatePlayersWithGameLobbies()
		{
			string query = @"
        UPDATE Player SET GameLobbyId = 1 WHERE PlayerId IN (1);
        UPDATE Player SET GameLobbyId = 2 WHERE PlayerId IN (2, 3);
        UPDATE Player SET GameLobbyId = 3 WHERE PlayerId IN (4,5,6);";

			_testDatabaseHelper.RunTransactionQuery(query);
		}

		private void AssosiateTooManyPlayersWithGameLobby()
		{
			string query = @"
        UPDATE Player SET GameLobbyId = 1 WHERE PlayerId IN (1,2,3,4,5,6);";

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

		private void SetTooManyOwnershipOfGameLobbies()
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
			AssosiatePlayersWithGameLobbies();
			SetValidOwnershipOfGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

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
		public void GetAllGameLobbies_TC2_ReturnsEmptyListIfNoLobbiesCouldBeFound()
		{
			// Arrange
			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.True(testResult != null, "Test result is null.");
			Assert.True(testResult.Count() == 0, "Test result count is not 0.");
		}


		[Fact]
		public void GetAllGameLobbies_TC3_AssignsAnOwnerIfNoOwnerCouldBeFound()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiatePlayersWithGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.NotNull(testResult);

			foreach (var gameLobby in testResult)
			{
				Assert.NotEqual(0, gameLobby.GameLobbyId);
				Assert.NotNull(gameLobby.LobbyName);
				Assert.NotNull(gameLobby.InviteLink);
				Assert.NotNull(gameLobby.PlayersInLobby);
				Assert.True(gameLobby.PlayersInLobby.Count(player => player.IsOwner) == 1); // Check that there is exactly one owner
				Assert.True(gameLobby.PlayersInLobby.All(player => !string.IsNullOrEmpty(player.InGameName))); // Check that all players have an InGameName
				Assert.True(gameLobby.PlayersInLobby.Count <= gameLobby.AmountOfPlayers);
			}
		}

		[Fact]
		public void GetAllGameLobbies_TC3_OwnerStatusOfRandomPlayersGetsUpdatedInDatabaseIfNoOwnerWasFound()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiatePlayersWithGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				foreach (var gameLobby in testResult)
				{
					// Query below selects the amount of players who's GameLobbyId matches the current gameLobby and is an owner
					var query = $"SELECT COUNT(*) FROM Player WHERE GameLobbyID = {gameLobby.GameLobbyId} AND IsOwner = 1";
					var ownerCount = connection.QuerySingle<int>(query);

					Assert.Equal(1, ownerCount);
				}
			}
		}

		[Fact]
		public void GetAllGameLobbies_TC4_RemovesOwnershipIfMultipleOwnersWasFound()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiatePlayersWithGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.True(testResult != null, "Test result is null.");

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

		public void GetAllGameLobbies_TC4_OwnerStatusOfRandomPlayersGetsUpdatedInDatabaseIfTooManyOwnersWasFound()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiatePlayersWithGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				foreach (var gameLobby in testResult)
				{
					// Query below selects the amount of players who's GameLobbyId matches the current gameLobby and is an owner
					var query = $"SELECT COUNT(*) FROM Player WHERE GameLobbyID = {gameLobby.GameLobbyId} AND IsOwner = 1";
					var ownerCount = connection.QuerySingle<int>(query);

					Assert.Equal(1, ownerCount);
				}
			}
		}


		[Fact]
		public void GetAllGameLobbies_TC5_DoesNotIncludeGameLobbiesWithNoPlayers()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.True(testResult != null, "Test result is null.");
			Assert.True(testResult.Count() == 0, "Test result count is not 0.");
		}

		[Fact]
		public void GetAllGameLobbies_TC5_DeletesGameLobbiesWithNoPlayers()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var query = "SELECT COUNT(*) FROM GameLobby";
				var gameLobbiesWithoutPlayers = connection.QuerySingle<int>(query);

				Assert.Equal(0, gameLobbiesWithoutPlayers);
			}
		}

		[Fact]
		public void GetAllGameLobbies_TC6_RemovesPlayersIfTooManyWasFound()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiateTooManyPlayersWithGameLobby();
			SetValidOwnershipOfGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.True(testResult != null, "Test result is null.");
			Assert.True(testResult.Count() == 1, "Test result count is not 1.");

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
		public void GetAllGameLobbies_TC6_UpdatesPlayersGameLobbyIdIfRemovedFromLobbyWithTooManyPlayers()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayersInGameLobbies();
			AssosiateTooManyPlayersWithGameLobby();
			SetValidOwnershipOfGameLobbies();

			GameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			PlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);
			PlayerLogic playerService = new PlayerLogic(_configuration, playerDatabaseAccessor);
			GameLobbyLogic SUT = new GameLobbyLogic(_configuration, gameLobbyDatabaseAccessor, playerService);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();

				var query = @"SELECT COUNT(*) 
                      FROM Player 
                      WHERE GameLobbyID IS NULL";
				var playersWithoutGameLobby = connection.QuerySingle<int>(query);

				// Expects there to be five players who is not in a lobby because the inserts assosiates six
				// players with gamelobby1 which has an AmountOfPlayers of 1
				Assert.True(playersWithoutGameLobby == 5);
			}
		}
	}
}
