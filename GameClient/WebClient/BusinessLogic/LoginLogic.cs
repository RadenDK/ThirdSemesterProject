using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using WebClient.Services;

namespace WebClient.BusinessLogic
{
    public class LoginLogic : ILoginLogic
    {
        private readonly IHttpClientService _httpClientService;

        public LoginLogic(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }

        public async Task<HttpResponseMessage> VerifyCredentials(string username, string password)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { Username = username, Password = password }), Encoding.UTF8, "application/json");
            return await _httpClientService.PostAsync("Player/verify", content);
        }

        public ClaimsPrincipal CreatePrincipal(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(identity);
        }

        //public void StorePlayerInSession(ISession session, PlayerModel player)
        //{
        //    session.SetString("Player", JsonSerializer.Serialize(player));
        //    session.SetString("InGameName", player.InGameName);
        //}
    }
}
