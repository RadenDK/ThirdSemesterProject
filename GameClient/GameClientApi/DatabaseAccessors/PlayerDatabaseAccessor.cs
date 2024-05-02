using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GameClientApi.DatabaseAccessors
{
	public class PlayerDatabaseAccessor : IPlayerDatabaseAccessor
	{

		private readonly string _connectionString;

		public PlayerDatabaseAccessor(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public string? GetPassword(string userName)
		{
			string selectQueryString = "SELECT PasswordHash FROM Player WHERE Username = @UserName";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var password = connection.QuerySingleOrDefault<string>(selectQueryString, new { UserName = userName });
				return password;
			}
		}

		public PlayerModel GetPlayer(string userName)
		{
			string selectQueryString = "SELECT * FROM Player WHERE Username = @UserName";
			string updateQueryString = "UPDATE Player SET OnlineStatus = 1 WHERE Username = @UserName";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				connection.Execute(updateQueryString, new { UserName = userName });
				var player = connection.QuerySingleOrDefault<PlayerModel>(selectQueryString, new { UserName = userName });
				return player;
			}
		}

		public bool CreatePlayer(AccountRegistrationModel newPlayer)
		{
			bool playerInserted = false;

			if (AccountHasValues(newPlayer))
			{
				string insertQuery = "INSERT INTO Player (Username, PasswordHash, InGameName, Email, Birthday) " +
					"VALUES (@Username, @Password, @InGameName, @Email, @Birthday)";

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					connection.Open();
					var rowsAffected = connection.Execute(insertQuery, newPlayer);
					playerInserted = rowsAffected == 1;
				}
			}

			return playerInserted;
		}

		private bool AccountHasValues(AccountRegistrationModel newAccount)
		{
			if (newAccount == null)
			{
				return false;
			}

			if (string.IsNullOrEmpty(newAccount.Username))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.Password))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.Email))
			{
				return false;
			}
			if (string.IsNullOrEmpty(newAccount.InGameName))
			{
				return false;
			}
			if (newAccount.BirthDay == null || newAccount.BirthDay < new DateTime(1753, 1, 1) || newAccount.BirthDay > new DateTime(9999, 12, 31))
			{
				return false;
			}

			return true;
		}

		public bool UsernameExists(string username)
		{
			string selectQueryString = "SELECT 1 FROM Player WHERE Username = @UserName";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var player = connection.QuerySingleOrDefault<string>(selectQueryString, new { UserName = username });
				return player != null;
			}
		}

		public bool InGameNameExists(string inGameName)
		{
			string selectQueryString = "SELECT 1 FROM Player WHERE InGameName = @inGameName";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				var player = connection.QuerySingleOrDefault<string>(selectQueryString, new { InGameName = inGameName });
				return player != null;
			}
		}

		public List<PlayerModel> GetAllPlayersInLobby(int? lobbyID, SqlTransaction transaction = null)
		{
			string getAllPlayersInLobbyQuery = "SELECT PlayerID, InGameName, IsOwner FROM Player WHERE GameLobbyID = @LobbyID";

			IDbConnection connection;
			if (transaction != null)
			{
				connection = transaction.Connection;
			}
			else
			{
				connection = new SqlConnection(_connectionString);
				connection.Open();
			}

			List<PlayerModel> players = connection.Query<PlayerModel>(getAllPlayersInLobbyQuery, new { LobbyID = lobbyID }, transaction: transaction).ToList();

			if (transaction == null)
			{
				connection.Close();
			}

			return players;

		}

		public bool UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel, SqlTransaction transaction = null)
		{
			string updatePlayerLobbyIdQuery = "UPDATE Player SET GameLobbyId = @GameLobbyId WHERE PlayerId = @PlayerId";

			IDbConnection connection;
			if (transaction != null)
			{
				connection = transaction.Connection;
			}
			else
			{
				connection = new SqlConnection(_connectionString);
				connection.Open();
			}

			int rowsAffected = connection.Execute(updatePlayerLobbyIdQuery, new
			{
				GameLobbyId = newGameLobbyModel.GameLobbyId,
				PlayerId = player.PlayerId
			}, transaction: transaction);

			if (transaction == null)
			{
				connection.Close();
			}

			return rowsAffected > 0;
		}

		public bool UpdatePlayerOwnership(PlayerModel player, SqlTransaction transaction = null)
		{
			string updateOwnershipQuery = "UPDATE Player SET IsOwner = @IsOwner WHERE PlayerId = @PlayerId";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				int rowsAffected = connection.Execute(updateOwnershipQuery, new { IsOwner = player.IsOwner, PlayerId = player.PlayerId });
				return rowsAffected > 0;
			}
		}

		public bool BanPlayer(int id)
		{
			string banPlayerQuery = "UPDATE Player SET Banned = @Banned WHERE PlayerId = @PlayerId";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				int rowsAffected = connection.Execute(banPlayerQuery, new { Banned = true, Id = id });
                return rowsAffected > 0;
            }
		}


		public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			connection.Open();
			SqlTransaction transaction = connection.BeginTransaction(isolationLevel);
			return transaction;
		}


		public void CommitTransaction(SqlTransaction sqlTransaction)
		{
			try
			{
				sqlTransaction.Commit();
			}
			finally
			{
				try
				{
					sqlTransaction.Connection.Close();
				}
				catch (NullReferenceException ex)
				{

				}
			}
		}

		public void RollbackTransaction(SqlTransaction sqlTransaction)
		{
			try
			{
				sqlTransaction.Rollback();
			}
			finally
			{
				try
				{
					sqlTransaction.Connection.Close();
				}
				catch (NullReferenceException ex)
				{

				}
			}
		}


	}
}
