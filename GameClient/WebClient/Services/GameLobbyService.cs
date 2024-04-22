﻿using Newtonsoft.Json;
using WebClient.Models;

namespace WebClient.Services
{
	public class GameLobbyService : IGameLobbyService
	{
		private readonly IHttpClientService _httpClientService;

		public GameLobbyService(IHttpClientService httpClientService)
		{
			_httpClientService = httpClientService;
		}

		public async Task<List<GameLobbyModel>> GetAllGameLobbies()
		{
			string endpoint = "GameLobby/AllGameLobbies";

			HttpResponseMessage response = await _httpClientService.GetAsync(endpoint);

			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				List<GameLobbyModel> gameLobbies = JsonConvert.DeserializeObject<List<GameLobbyModel>>(responseBody);
				return gameLobbies;
			}
			else
			{
				throw new Exception($"Failed to get game lobbies. HTTP status code: {response.StatusCode}");
			}
		}



		public async Task<GameLobbyModel> GetGameLobbyById(int lobbyId)
		{
			return new GameLobbyModel();
		}
	}
}