﻿using GameClientApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GameClientApi.DatabaseAccessors
{
	public interface IPlayerDatabaseAccessor
	{

		bool SetOnlineStatus(PlayerModel player, SqlTransaction transaction = null);

        bool CreatePlayer(AccountRegistrationModel newPlayer);

		bool UsernameExists(string username);

		bool InGameNameExists(string ingamename);

		List<PlayerModel> GetAllPlayersInLobby(int? lobbyID, SqlTransaction transaction = null);

		bool UpdatePlayerLobbyId(PlayerModel player, GameLobbyModel newGameLobbyModel, SqlTransaction transaction = null);

		bool UpdatePlayerOwnership(PlayerModel player, SqlTransaction transaction = null);

		PlayerModel GetPlayer(string username = null, int? playerId = null, SqlTransaction transaction = null);

		List<PlayerModel> GetAllPlayers();

		SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

		void CommitTransaction(SqlTransaction sqlTransaction);

		void RollbackTransaction(SqlTransaction sqlTransaction);

		bool UpdatePlayer(PlayerModel player, SqlTransaction transaction);

        bool DeletePlayer(int? playerId);
    }
}
