using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
	public class GameLobbyDatabaseAccessor : IGameLobbyDatabaseAccessor
	{
		private IPlayerDatabaseAccessor _playerDatabaseAccessor;
		private readonly string _connectionString;
		public GameLobbyDatabaseAccessor(IConfiguration configuration)
		{
			_playerDatabaseAccessor = new PlayerDatabaseAccessor(configuration);
			_connectionString = configuration.GetConnectionString("DefaultConnection");

		}

		public List<GameLobbyModel> GetAllGameLobbies()
		{
			List<GameLobbyModel> gameLobbies = new List<GameLobbyModel>();
			string getAllGameLobbies = "SELECT GameLobbyID, LobbyName, AmountOfPlayers, PasswordHash, InviteLink FROM GameLobby";
			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				gameLobbies = connection.Query<GameLobbyModel>(getAllGameLobbies).ToList();
			}
			foreach (GameLobbyModel gameLobby in gameLobbies)
			{
				List<PlayerModel> playersInLobby = _playerDatabaseAccessor.GetAllPlayersInLobby(gameLobby.GameLobbyId);
				gameLobby.PlayersInLobby = playersInLobby;
				SetOwnerOfLobby(gameLobby);
			}

			return gameLobbies;

		}
		private void SetOwnerOfLobby(GameLobbyModel gameLobby)
		{
			foreach (PlayerModel player in gameLobby.PlayersInLobby)
			{
				if (player.IsOwner)
				{
					gameLobby.Owner = player;
				}
			}
		}
	}


}
