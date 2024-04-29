﻿using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using BC = BCrypt.Net.BCrypt;
namespace GameClientApi.BusinessLogic
{
	public class PlayerLogic : IPlayerLogic
	{
		IPlayerDatabaseAccessor _playerAccessor;
		ITransactionHandler _transactionHandler;

		public PlayerLogic(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor, ITransactionHandler transactionHandler)
		{
			_playerAccessor = playerDatabaseAccessor;
            _transactionHandler = transactionHandler;
        }

		public bool VerifyLogin(string userName, string password)
		{
			string? storedHashedPassword = _playerAccessor.GetPassword(userName);
			if (storedHashedPassword == null || userName == null)
			{
				throw new ArgumentNullException("Stored HashedPassword or username is null");
			}
			return BC.Verify(password, storedHashedPassword);
		}

		public PlayerModel GetPlayer(string userName)
		{
			PlayerModel playerData = _playerAccessor.GetPlayer(userName);
			if (playerData == null)
			{
				throw new Exception("Player not found");
			}
			return playerData;
		}


		public bool CreatePlayer(AccountRegistrationModel newPlayerAccount)
		{
			if (_playerAccessor.UsernameExists(newPlayerAccount.Username))
			{
				throw new ArgumentException("Username already exists");
			}
			if (_playerAccessor.InGameNameExists(newPlayerAccount.InGameName))
			{
				throw new ArgumentException("InGameName already exists");
			}

			AccountRegistrationModel newPlayerAccountWithHashedPassword = new AccountRegistrationModel
			{
				Username = newPlayerAccount.Username,
				Password = BC.HashPassword(newPlayerAccount.Password),
				Email = newPlayerAccount.Email,
				InGameName = newPlayerAccount.InGameName,
				BirthDay = newPlayerAccount.BirthDay
			};

			return _playerAccessor.CreatePlayer(newPlayerAccountWithHashedPassword);
		}

		public List<PlayerModel> GetAllPlayersInLobby(int? lobbyId, SqlTransaction transaction = null)
		{
			return _playerAccessor.GetAllPlayersInLobby(lobbyId, transaction);

		}

		public void UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel)
		{
			SqlTransaction transaction = _transactionHandler.BeginTransaction(IsolationLevel.ReadUncommitted);

			_playerAccessor.UpdatePlayerLobbyId(player, newGameLobbyModel, transaction);

			if (!TooManyPlayersInLobby(newGameLobbyModel, transaction))
			{
				_transactionHandler.CommitTransaction(transaction);

			}
			else
			{
				_transactionHandler.RollbackTransaction(transaction);
				throw new ArgumentException("Too many players in lobby");
			}
		}

		private bool TooManyPlayersInLobby(GameLobbyModel gameLobby, SqlTransaction transaction)
		{
			List<PlayerModel> playersInGameLobby = GetAllPlayersInLobby(gameLobby.GameLobbyId, transaction);

			bool lobbyHasTooManyPlayers = playersInGameLobby.Count > gameLobby.AmountOfPlayers;

			return lobbyHasTooManyPlayers;

		}

		public void UpdatePlayerOwnership(PlayerModel player)
		{
			_playerAccessor.UpdatePlayerOwnership(player);

		}


	}
}
