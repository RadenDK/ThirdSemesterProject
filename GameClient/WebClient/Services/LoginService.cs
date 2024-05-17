
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using WebClient.Models;

namespace WebClient.Services
{
    public class LoginService : ILoginService
    {
        private readonly IHttpClientService _httpClientService;

        public LoginService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public async Task<HttpResponseMessage> VerifyPlayerCredentials(string username, string password, string accessToken)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(new { Username = username, Password = password }), Encoding.UTF8, "application/json");

            _httpClientService.SetAuthenticationHeader(accessToken);

            return await _httpClientService.PostAsync("Player/verify", content);
        }

        public async Task<HttpResponseMessage> LogoutAsync(int playerId, string accessToken)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(new { PlayerId = playerId } ), Encoding.UTF8, "application/json");
            _httpClientService.SetAuthenticationHeader(accessToken);
            HttpResponseMessage response = await _httpClientService.PostAsync("Player/logout", content);
            return response;
        }

        public async Task<PlayerModel> GetPlayerAsync(string username, string accessToken)
        {
            _httpClientService.SetAuthenticationHeader(accessToken);
            HttpResponseMessage response = await _httpClientService.GetAsync($"Player/{username}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                PlayerModel player = JsonConvert.DeserializeObject<PlayerModel>(responseBody);
                return player;
            }
            else
            {
                throw new Exception($"Failed to get player. HTTP status code: {response.StatusCode}");
            }
        }
	}
}
