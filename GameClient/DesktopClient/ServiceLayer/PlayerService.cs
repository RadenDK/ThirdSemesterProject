using DesktopClient.ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DesktopClient.Services;

namespace DesktopClient.ServiceLayer
{
	public class PlayerService : IPlayerService
	{
		private readonly IHttpClientService _httpClientService;

		public PlayerService(IHttpClientService httpClientService)
		{
			_httpClientService = httpClientService;
		}

        public PlayerService()
        {
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

		public async Task<bool> BanPlayer(string username)
		{
			string endpoint = "Player/ban";

			StringContent content = new StringContent(
				JsonConvert.SerializeObject(new { username = username }),
				Encoding.UTF8,
				"application/json");

			HttpResponseMessage response = await _httpClientService.PatchAsync(endpoint, content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
				return false;
                throw new Exception($"Failed to ban player. HTTP status code: {response.StatusCode}");
            }
        }
	}
}
