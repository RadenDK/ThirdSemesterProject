﻿using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameClientApi.Services
{
	public class GameLobbyService : IGameLobbyService
	{
		private readonly IGameLobbyDatabaseAccessor _gameLobbyAccessor;
		private readonly IPlayerService _playerService;

		public GameLobbyService(IConfiguration configuration,
			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor,
			IPlayerService playerService)
		{
			_gameLobbyAccessor = gameLobbyDatabaseAccessor;
			_playerService = playerService;
		}

		public IEnumerable<GameLobbyModel> GetAllGameLobbies()
		{
			List<GameLobbyModel> gameLobbies = _gameLobbyAccessor.GetAllGameLobbies();
			return gameLobbies.Select(InitializeAndValidateGameLobby).Where(gameLobby => gameLobby != null);
		}

		private GameLobbyModel InitializeAndValidateGameLobby(GameLobbyModel gameLobby)
		{
			gameLobby.PlayersInLobby = _playerService.GetAllPlayersInLobby(gameLobby.GameLobbyId);

			if (IsValidGameLobby(gameLobby))
			{
				return gameLobby;
			}

			return MakeGameLobbyValid(gameLobby);
		}

		private bool IsValidGameLobby(GameLobbyModel gameLobby)
		{
			return HasPlayers(gameLobby) &&
				   HasSingleOwner(gameLobby) &&
				   HasValidPlayerCount(gameLobby);
		}

		private bool HasPlayers(GameLobbyModel gameLobby)
		{
			return gameLobby.PlayersInLobby.Any();
		}

		private bool HasSingleOwner(GameLobbyModel gameLobby)
		{
			return gameLobby.PlayersInLobby.Count(player => player.IsOwner) == 1;
		}

		private bool HasValidPlayerCount(GameLobbyModel gameLobby)
		{
			return gameLobby.PlayersInLobby.Count <= gameLobby.AmountOfPlayers;
		}

		private GameLobbyModel MakeGameLobbyValid(GameLobbyModel gameLobby)
		{
			if (!HasPlayers(gameLobby))
			{
				_gameLobbyAccessor.DeleteGameLobby(gameLobby.GameLobbyId);
				return null;
			}

			if (!HasSingleOwner(gameLobby))
			{
				AssignOwnerToLobbyAndUpdateDatabase(gameLobby);
			}

			if (HasTooManyOwners(gameLobby))
			{
				RemoveOwnersUntilOneIsLeftAndUpdateDatabase(gameLobby);
			}

			if (!HasValidPlayerCount(gameLobby))
			{
				KickPlayersUntilLobbyCapacityIsMet(gameLobby);
			}

			return gameLobby;
		}

		private bool HasTooManyOwners(GameLobbyModel gameLobby)
		{
			return gameLobby.PlayersInLobby.Count(player => player.IsOwner) > 1;
		}

		private void AssignOwnerToLobbyAndUpdateDatabase(GameLobbyModel gameLobby)
		{
			PlayerModel firstPlayer = gameLobby.PlayersInLobby.First();
			firstPlayer.IsOwner = true;
			_playerService.UpdatePlayerLobbyId(firstPlayer);
		}

		private void KickPlayersUntilLobbyCapacityIsMet(GameLobbyModel gameLobby)
		{
			List<PlayerModel> playersToKick = gameLobby.PlayersInLobby
				.Where(player => !player.IsOwner)
				.Take(gameLobby.PlayersInLobby.Count - gameLobby.AmountOfPlayers)
				.ToList();

			foreach (PlayerModel player in playersToKick)
			{
				gameLobby.PlayersInLobby.Remove(player);
				_playerService.UpdatePlayerLobbyId(player);
			}
		}

		private void RemoveOwnersUntilOneIsLeftAndUpdateDatabase(GameLobbyModel gameLobby)
		{
			List<PlayerModel> owners = gameLobby.PlayersInLobby.Where(player => player.IsOwner).Skip(1).ToList();
			foreach (PlayerModel owner in owners)
			{
				owner.IsOwner = false;
				_playerService.UpdatePlayerOwnership(owner);
			}
		}
	}
}
