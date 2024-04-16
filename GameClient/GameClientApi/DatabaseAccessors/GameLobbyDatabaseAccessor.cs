﻿using Dapper;
using GameClientApi.Models;
using Microsoft.Data.SqlClient;

namespace GameClientApi.DatabaseAccessors
{
	public class GameLobbyDatabaseAccessor : IGameLobbyDatabaseAccessor
	{
		private readonly string _connectionString;
		public GameLobbyDatabaseAccessor(IConfiguration configuration)
		{
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
			return gameLobbies;
		}

		public bool DeleteGameLobby(int gameLobbyId)
		{
			bool deletionSucces = false;

			string deleteLobbyQuery = "DELETE FROM GameLobby WHERE GameLobbyId = @GameLobbyId";

			using(SqlConnection connection = new SqlConnection(_connectionString))
			{
				connection.Open();
				int rowsAffected = connection.Execute(deleteLobbyQuery, new {GameLobbyId = gameLobbyId});
				if (rowsAffected > 0) deletionSucces = true;	
			}

			return deletionSucces;
		}
	}


}