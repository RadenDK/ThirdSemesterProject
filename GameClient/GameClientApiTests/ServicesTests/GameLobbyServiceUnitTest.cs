using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GameClientApi.DatabaseAccessors;
using GameClientApi.Services;
using GameClientApi.Models;
using GameClientApiTests.TestHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Moq;

namespace GameClientApiTests.ServicesTests
{
	public class GameLobbyServiceUnitTest
	{

		private readonly IConfiguration? _mockConfiguration;
		private readonly Mock<IGameLobbyDatabaseAccessor> _gameLobbyMockAccessor;
		private readonly Mock<IPlayerService> _playerMockService;


		public GameLobbyServiceUnitTest()
		{
			_gameLobbyMockAccessor = new Mock<IGameLobbyDatabaseAccessor>();
			_playerMockService = new Mock<IPlayerService>();
		}


		[Fact]
		public void GetAllGameLobbies_TC1_ReturnsExpectedGameLobbies()
		{
			// Arrange
			List<GameLobbyModel> expectedGameLobbies = new List<GameLobbyModel>
			{
				new GameLobbyModel { GameLobbyId = 1, LobbyName = "Lobby1", AmountOfPlayers = 1, InviteLink = "inviteLink1" },
				new GameLobbyModel { GameLobbyId = 2, LobbyName = "Lobby2", AmountOfPlayers = 2, InviteLink = "inviteLink2" },
			};
			_gameLobbyMockAccessor.Setup(a => a.GetAllGameLobbies())
				.Returns(expectedGameLobbies);

			List<PlayerModel> expectedPlayersGameLobby1 = new List<PlayerModel>
			{
				new PlayerModel { UserName = "Player1", InGameName = "InGame1", IsOwner = true },
			};

			List<PlayerModel> expectedPlayersGameLobby2 = new List<PlayerModel>
			{
				new PlayerModel { UserName = "Player4", InGameName = "InGame4", IsOwner = true },
				new PlayerModel { UserName = "Player5", InGameName = "InGame5", IsOwner = false },
			};


			_playerMockService.Setup(a => a.GetAllPlayersInLobby(1)).Returns(expectedPlayersGameLobby1);
			_playerMockService.Setup(a => a.GetAllPlayersInLobby(2)).Returns(expectedPlayersGameLobby2);


			GameLobbyService SUT = new GameLobbyService(_mockConfiguration,
				_gameLobbyMockAccessor.Object, _playerMockService.Object);

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
			List<GameLobbyModel> expectedGameLobbies = new List<GameLobbyModel>();

			_gameLobbyMockAccessor.Setup(a => a.GetAllGameLobbies())
				.Returns(expectedGameLobbies);

			GameLobbyService SUT = new GameLobbyService(_mockConfiguration,
				_gameLobbyMockAccessor.Object, _playerMockService.Object);

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
			List<GameLobbyModel> expectedGameLobbies = new List<GameLobbyModel>
			{
				new GameLobbyModel { GameLobbyId = 1, LobbyName = "Lobby1", AmountOfPlayers = 1, InviteLink = "inviteLink1" },
			};
			_gameLobbyMockAccessor.Setup(a => a.GetAllGameLobbies())
				.Returns(expectedGameLobbies);

			List<PlayerModel> expectedPlayersGameLobby1 = new List<PlayerModel>
			{
				new PlayerModel { UserName = "Player1", InGameName = "InGame1", IsOwner = false },
				new PlayerModel { UserName = "Player2", InGameName = "InGame2", IsOwner = false },
			};

			_playerMockService.Setup(a => a.GetAllPlayersInLobby(1)).Returns(expectedPlayersGameLobby1);

			GameLobbyService SUT = new GameLobbyService(_mockConfiguration,
				_gameLobbyMockAccessor.Object, _playerMockService.Object);

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
		public void GetAllGameLobbies_TC4_RemovesOwnershipIfMultipleOwnersWasFound()
		{
			// Arrange
			List<GameLobbyModel> expectedGameLobbies = new List<GameLobbyModel>
			{
				new GameLobbyModel { GameLobbyId = 1, LobbyName = "Lobby1", AmountOfPlayers = 2, InviteLink = "inviteLink1" },
			};
			_gameLobbyMockAccessor.Setup(a => a.GetAllGameLobbies())
				.Returns(expectedGameLobbies);

			List<PlayerModel> expectedPlayersGameLobby1 = new List<PlayerModel>
			{
				new PlayerModel { UserName = "Player1", InGameName = "InGame1", IsOwner = true },
				new PlayerModel { UserName = "Player2", InGameName = "InGame2", IsOwner = true },
			};

			_playerMockService.Setup(a => a.GetAllPlayersInLobby(1)).Returns(expectedPlayersGameLobby1);
			
			GameLobbyService SUT = new GameLobbyService(_mockConfiguration,
				_gameLobbyMockAccessor.Object, _playerMockService.Object);

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
		public void GetAllGameLobbies_TC5_DoesIncludeGameLobbiesWithNoPlayers()
		{
			// Arrange
			List<GameLobbyModel> expectedGameLobbies = new List<GameLobbyModel>
			{
				new GameLobbyModel { GameLobbyId = 1, LobbyName = "Lobby1", AmountOfPlayers = 1, InviteLink = "inviteLink1" },
			};
			_gameLobbyMockAccessor.Setup(a => a.GetAllGameLobbies())
				.Returns(expectedGameLobbies);

			List<PlayerModel> expectedPlayersGameLobby1 = new List<PlayerModel>();

			_playerMockService.Setup(a => a.GetAllPlayersInLobby(1)).Returns(expectedPlayersGameLobby1);

			GameLobbyService SUT = new GameLobbyService(_mockConfiguration,
				_gameLobbyMockAccessor.Object, _playerMockService.Object);

			// Act
			IEnumerable<GameLobbyModel> testResult = SUT.GetAllGameLobbies();

			// Assert
			Assert.True(testResult != null, "Test result is null.");
			Assert.True(testResult.Count() == 0, "Test result count is not 0.");
		}

		[Fact]
		public void GetAllGameLobbies_TC6_RemovesPlayersIfTooManyWasFound()
		{
			// Arrange
			List<GameLobbyModel> expectedGameLobbies = new List<GameLobbyModel>
			{
				new GameLobbyModel { GameLobbyId = 1, LobbyName = "Lobby1", AmountOfPlayers = 2, InviteLink = "inviteLink1" },
			};
			_gameLobbyMockAccessor.Setup(a => a.GetAllGameLobbies())
				.Returns(expectedGameLobbies);

			List<PlayerModel> expectedPlayersGameLobby1 = new List<PlayerModel>
			{
				new PlayerModel { UserName = "Player1", InGameName = "InGame1", IsOwner = true },
				new PlayerModel { UserName = "Player2", InGameName = "InGame2", IsOwner = false },
				new PlayerModel { UserName = "Player3", InGameName = "InGame3", IsOwner = false },

			};

			_playerMockService.Setup(a => a.GetAllPlayersInLobby(1)).Returns(expectedPlayersGameLobby1);

			GameLobbyService SUT = new GameLobbyService(_mockConfiguration,
				_gameLobbyMockAccessor.Object, _playerMockService.Object);

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
	}
}
