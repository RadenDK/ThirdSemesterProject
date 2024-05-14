using GameClientApi.DatabaseAccessors;
using Microsoft.Extensions.Configuration;
using GameClientApi.Controllers;
using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc;
using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameClientApiTests.ControllerTests
{
	[Collection("Sequential")]
	public class GameLobbyControllerIntergrationTest : IDisposable
	{

		private IConfiguration _configuration;

		private string _connectionString;

		private TestDatabaseHelper _testDatabaseHelper;


		public GameLobbyControllerIntergrationTest()
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
			(3, 'LobbyNameTest3' , '$2a$11$6jP86mmF8jNKzzzYV/H85OYX.c4EF3d285sJGC0s5cn/X9fFR86hG', 3, 'InviteLinkTest3', 3);
							
			SET IDENTITY_INSERT GameLobby OFF;";

			_testDatabaseHelper.RunTransactionQuery(query);
		}

		private void InsertMockPlayers()
		{
			string query = @"
			SET IDENTITY_INSERT Player ON;

        INSERT INTO Player (PlayerId, Username, PasswordHash, InGameName, Birthday, IsOwner, GameLobbyId)
        VALUES (1, 'Player1', 'PasswordHash1', 'InGameName1', '2022-01-01', 1, 1),
               (2, 'Player2', 'PasswordHash2', 'InGameName2', '2022-01-02', 1, 2),
               (3, 'Player3', 'PasswordHash3', 'InGameName3', '2022-01-03', 0, null),
               (4, 'Player4', 'PasswordHash4', 'InGameName4', '2022-01-04', 1, 3),
               (5, 'Player5', 'PasswordHash5', 'InGameName5', '2022-01-05', 0, 3),
               (6, 'Player6', 'PasswordHash6', 'InGameName6', '2022-01-06', 0, null);";

			_testDatabaseHelper.RunTransactionQuery(query);
		}

		[Fact]
		public void JoinGameLobby_TC1_ReturnsIActionResultWithGameLobbyInDatabaseWithoutPassword()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayers();

			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequestModel joinRequestModel = new JoinGameLobbyRequestModel
			{
				PlayerId = 6,
				GameLobbyId = 2,
				LobbyPassword = null
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequestModel);

			// Assert
			Assert.True(testResult is OkObjectResult, "The result should be of type OkObjectResult.");
			OkObjectResult okResult = (OkObjectResult)testResult;

			Assert.True(okResult.Value is GameLobbyModel, "The value in the result should be of type GameLobbyModel.");
			GameLobbyModel returnValue = (GameLobbyModel)okResult.Value;

			Assert.True(joinRequestModel.GameLobbyId == returnValue.GameLobbyId, "The GameLobbyId in the response does not match the GameLobbyId in the request.");

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT GameLobbyId FROM Player WHERE PlayerId = @PlayerId", connection))
				{
					command.Parameters.AddWithValue("@PlayerId", joinRequestModel.PlayerId);
					var gameLobbyId = (int)command.ExecuteScalar();
					Assert.True(joinRequestModel.GameLobbyId == gameLobbyId, "The GameLobbyId in the database does not match the GameLobbyId in the request.");
				}
			}
		}

		[Fact]
		public void JoinGameLobby_TC2_ReturnsIActionResultWithGamelobbyForLobbyInDatabaseWithPassword()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayers();

			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequestModel joinRequestModel = new JoinGameLobbyRequestModel
			{
				PlayerId = 6,
				GameLobbyId = 3,
				LobbyPassword = "password"
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequestModel);

			// Assert
			OkObjectResult okResult = Assert.IsType<OkObjectResult>(testResult);
			GameLobbyModel returnValue = Assert.IsType<GameLobbyModel>(okResult.Value);

			Assert.Equal(joinRequestModel.GameLobbyId, returnValue.GameLobbyId);

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT GameLobbyId FROM Player WHERE PlayerId = @PlayerId", connection))
				{
					command.Parameters.AddWithValue("@PlayerId", joinRequestModel.PlayerId);
					var gameLobbyId = (int)command.ExecuteScalar();
					Assert.True(joinRequestModel.GameLobbyId == gameLobbyId, "The GameLobbyId in the database does not match the GameLobbyId in the request.");
				}
			}
		}

		[Fact]
		public void JoinGameLobby_TC3_ReturnsBadRequestWhenGamelobbyDoesNotExist()
		{
			// Arrange
			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequestModel joinRequestModel = new JoinGameLobbyRequestModel
			{
				PlayerId = 6,
				GameLobbyId = 999,
				LobbyPassword = null
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequestModel);

			// Assert
			Assert.IsType<BadRequestObjectResult>(testResult);

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT GameLobbyId FROM Player WHERE PlayerId = @PlayerId", connection))
				{
					command.Parameters.AddWithValue("@PlayerId", joinRequestModel.PlayerId);
					int? gameLobbyId = (int?)command.ExecuteScalar();
					Assert.True(gameLobbyId == null, "The GameLobbyId in the database does not match the GameLobbyId in the request.");
				}
			}
		}

		[Fact]
		public void JoinGameLobby_TC4_ReturnsBadRequestPasswordIsInvalid()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayers();

			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequestModel joinRequestModel = new JoinGameLobbyRequestModel
			{
				PlayerId = 6,
				GameLobbyId = 3, // Game lobby ID with a password
				LobbyPassword = "wrongpassword" // Incorrect password
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequestModel);

			// Assert
			Assert.IsType<BadRequestObjectResult>(testResult);
		}

		[Fact]
		public void JoinGameLobby_TC5_DeniesJoiningAFullLobbyWithoutPassword()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayers();

			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequestModel joinRequestModel = new JoinGameLobbyRequestModel
			{
				PlayerId = 6,
				GameLobbyId = 1,
				LobbyPassword = null
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequestModel);

			// Assert
			Assert.True(testResult is BadRequestObjectResult, "The result should be of type BadRequestObjectResult.");

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT GameLobbyId FROM Player WHERE PlayerId = @PlayerId", connection))
				{
					command.Parameters.AddWithValue("@PlayerId", joinRequestModel.PlayerId);
					var result = command.ExecuteScalar();
					int? gameLobbyId = result == DBNull.Value ? null : (int?)result;
					Assert.True(gameLobbyId == null, "The GameLobbyId in the database does not match the GameLobbyId in the request.");
				}
			}
		}

		[Fact]
		public void JoinGameLobby_TC6_DeniesOnePlayerFromJoiningALobbyWithOneSpaceOpenWhenTwoTryToJoinAtTheSameTime()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayers();

			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequestModel joinRequestModelPlayer1 = new JoinGameLobbyRequestModel
			{
				PlayerId = 1,
				GameLobbyId = 3,
				LobbyPassword = "password"
			};

			JoinGameLobbyRequestModel joinRequestModelPlayer2 = new JoinGameLobbyRequestModel
			{
				PlayerId = 2,
				GameLobbyId = 3,
				LobbyPassword = "password"
			};

			// Act
			IActionResult testResultPlayer1 = SUT.JoinGameLobby(joinRequestModelPlayer1);
			IActionResult testResultPlayer2 = SUT.JoinGameLobby(joinRequestModelPlayer2);

			// Assert
			// Check that one of the players received a BadRequest response
			Assert.True(testResultPlayer1 is BadRequestObjectResult || testResultPlayer2 is BadRequestObjectResult, "One of the players should have received a BadRequest response.");

			// Check that one of the players successfully joined the lobby
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Player WHERE GameLobbyId = @GameLobbyId", connection))
				{
					command.Parameters.AddWithValue("@GameLobbyId", joinRequestModelPlayer1.GameLobbyId);
					int playersInLobby = (int)command.ExecuteScalar();
					Assert.True(playersInLobby == 3, "There should be 3 players in the lobby.");
				}
			}

			// Check that one of the players has their GameLobbyId updated in the database
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT GameLobbyId FROM Player WHERE PlayerId = @PlayerId", connection))
				{
					command.Parameters.AddWithValue("@PlayerId", joinRequestModelPlayer1.PlayerId);
					int? gameLobbyIdPlayer1 = (int?)command.ExecuteScalar();

					command.Parameters.Clear();
					command.Parameters.AddWithValue("@PlayerId", joinRequestModelPlayer2.PlayerId);
					int? gameLobbyIdPlayer2 = (int?)command.ExecuteScalar();

					Assert.True(gameLobbyIdPlayer1 == joinRequestModelPlayer1.GameLobbyId || gameLobbyIdPlayer2 == joinRequestModelPlayer2.GameLobbyId, "One of the players should have their GameLobbyId updated in the database.");
				}
			}
		}

		[Fact]
		public void LeaveGameLobby_PlayerLeavesSuccessfully_WhenPlayerExistsInLobby()
		{
			// Arrange
			InsertMockGameLobbies();
			InsertMockPlayers();

			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			LeaveGameLobbyRequestModel leaveRequest = new LeaveGameLobbyRequestModel { GameLobbyId = 3, PlayerId = 5 };

			// Act
			IActionResult result = SUT.LeaveGameLobby(leaveRequest);

			// Assert
			Assert.IsType<OkResult>(result); // Assert that the result is an OkResult

			// Check that the player has left the lobby in the database
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand("SELECT GameLobbyId, IsOwner FROM Player WHERE PlayerId = @PlayerId", connection))
				{
					command.Parameters.AddWithValue("@PlayerId", leaveRequest.PlayerId);
					using (SqlDataReader reader = command.ExecuteReader())
					{
						if (reader.Read())
						{
							int? gameLobbyId = reader.IsDBNull(0) ? null : reader.GetInt32(0);
							bool isOwner = reader.GetBoolean(1);

							Assert.True(gameLobbyId == null, "GameLobbyId not null"); // Assert that the GameLobbyId is null
							Assert.True(isOwner == false, "Player is still gameowner"); // Assert that IsOwner is 0
						}
					}
				}
			}
		}
	}
}
