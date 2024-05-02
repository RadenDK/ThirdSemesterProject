using DesktopClient.ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebClient.Services;
using Newtonsoft.Json;

namespace DesktopClient.ServiceLayer
{
	public class PlayerService : IPlayerService
	{
		private readonly IHttpClientService _httpClientService;

		public PlayerService(IHttpClientService httpClientService)
		{
			_httpClientService = httpClientService;
		}

		public async Task<List<PlayerModel>> GetAllPlayers()
		{
			string endpoint = "Player/AllPlayers";

			HttpResponseMessage response = await _httpClientService.GetAsync(endpoint);
			if (response.IsSuccessStatusCode)
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				List<PlayerModel> allPlayers = JsonConvert.DeserializeObject<List<PlayerModel>>(responseBody);
				return allPlayers;
			}
			else
			{
				throw new Exception($"Failed to get players. HTTP status code: {response.StatusCode}");
			}
		}
	}
}
