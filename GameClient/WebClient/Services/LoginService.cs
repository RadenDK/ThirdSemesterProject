﻿
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
	}
}
