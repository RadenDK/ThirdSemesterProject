using Newtonsoft.Json;
using System.Net;
using System.Text;
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



		public async Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request)
		{
			string url = "GameLobby/Join";
			var json = JsonConvert.SerializeObject(request);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				var response = await _httpClientService.PostAsync(url, data);
				if (response.IsSuccessStatusCode)
				{
					var responseBody = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<GameLobbyModel>(responseBody);
				}
				else
				{
					throw new Exception($"Failed to join game lobby. HTTP status code: {response.StatusCode}");
				}
			}
			catch (HttpRequestException ex)
			{
				throw new Exception("An error occurred while trying to join the game lobby. Please try again later.", ex);
			}
		}

	}
}
