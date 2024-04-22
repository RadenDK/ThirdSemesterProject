using GameClientApi.DatabaseAccessors;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GameClientApi.Controllers;
using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc;
using GameClientApiTests.TestHelpers;

namespace GameClientApiTests.ControllerTests
{
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
               (3, 'Player3', 'PasswordHash3', 'InGameName3', '2022-01-03', 0, 2),
               (4, 'Player4', 'PasswordHash4', 'InGameName4', '2022-01-04', 1, 3),
               (5, 'Player5', 'PasswordHash5', 'InGameName5', '2022-01-05', 0, 3),
               (6, 'Player6', 'PasswordHash6', 'InGameName6', '2022-01-06', 0, 3);";

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

			JoinGameLobbyRequest joinRequest = new JoinGameLobbyRequest
			{
				GameLobbyId = 2, 
				Password = null 
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequest);

			// Assert
			OkObjectResult okResult = Assert.IsType<OkObjectResult>(testResult);
			GameLobbyModel returnValue = Assert.IsType<GameLobbyModel>(okResult.Value);

			Assert.Equal(joinRequest.GameLobbyId, returnValue.GameLobbyId);
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

			JoinGameLobbyRequest joinRequest = new JoinGameLobbyRequest
			{
				GameLobbyId = 3,
				Password = "password"
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequest);

			// Assert
			OkObjectResult okResult = Assert.IsType<OkObjectResult>(testResult);
			GameLobbyModel returnValue = Assert.IsType<GameLobbyModel>(okResult.Value);

			Assert.Equal(joinRequest.GameLobbyId, returnValue.GameLobbyId);
		}

		[Fact]
		public void JoinGameLobby_TC3_ReturnsBadRequestWhenGamelobbyDoesNotExist()
		{
			// Arrange
			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor = new GameLobbyDatabaseAccessor(_configuration);
			IPlayerDatabaseAccessor playerDatabaseAccessor = new PlayerDatabaseAccessor(_configuration);

			GameLobbyController SUT = new GameLobbyController(_configuration, gameLobbyDatabaseAccessor, playerDatabaseAccessor);

			JoinGameLobbyRequest joinRequest = new JoinGameLobbyRequest
			{
				GameLobbyId = 999, 
				Password = null
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequest);

			// Assert
			Assert.IsType<BadRequestObjectResult>(testResult);
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

			JoinGameLobbyRequest joinRequest = new JoinGameLobbyRequest
			{
				GameLobbyId = 3, // Game lobby ID with a password
				Password = "wrongpassword" // Incorrect password
			};

			// Act
			IActionResult testResult = SUT.JoinGameLobby(joinRequest);

			// Assert
			Assert.IsType<BadRequestObjectResult>(testResult);
		}


	}
}
