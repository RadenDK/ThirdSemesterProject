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

		public async Task<List<GameLobbyModel>> GetAllGameLobbies(string accessToken)
		{
			string endpoint = "GameLobby/AllGameLobbies";

			_httpClientService.SetAuthenticationHeader(accessToken);

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



		public async Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request, string accessToken)
		{
			string url = "GameLobby/Join";
			var json = JsonConvert.SerializeObject(request);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				_httpClientService.SetAuthenticationHeader(accessToken);

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

		public async Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username, string accessToken)
		{
			string endpoint = "GameLobby/CreateGameLobby";
			var payload = new { newLobby, username };
			StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

			_httpClientService.SetAuthenticationHeader(accessToken);

			HttpResponseMessage response = await _httpClientService.PostAsync(endpoint, jsonContent);
			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				GameLobbyModel gameLobby = JsonConvert.DeserializeObject<GameLobbyModel>(responseBody);
				return gameLobby;
			}
			else 
			{ 
				throw new Exception($"Failed to create game lobby. HTTP response code: {response.StatusCode}"); 
			}
		}

		public async Task<bool> LeaveGameLobby(int playerId, int gameLobbyId, string accessToken)
		{
			string endpoint = "GameLobby/Leave";
			
			LeaveGameLobbyRequestModel leaveRequest = new LeaveGameLobbyRequestModel { PlayerId = playerId, GameLobbyId = gameLobbyId };

			StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(leaveRequest), Encoding.UTF8, "application/json");

			_httpClientService.SetAuthenticationHeader(accessToken);
			
			HttpResponseMessage response = await _httpClientService.PostAsync(endpoint, jsonContent);
			
			if (response.IsSuccessStatusCode)
			{
				return true;
			}
			else
			{
				throw new Exception($"Failed leave lobby: {response.StatusCode}, response message: {response.Content}");
			}

		}
	}
}
