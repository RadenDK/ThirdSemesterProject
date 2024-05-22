using Microsoft.AspNetCore.Authentication.Cookies;
using System.Numerics;
using System.Security.Claims;
using System.Text.Json;
using WebClient.Models;
using WebClient.Security;
using WebClient.Services;

namespace WebClient.BusinessLogic
{
    public class LoginLogic : ILoginLogic
    {
        private readonly ILoginService _loginService;
        private ITokenManager _tokenManager;

        public LoginLogic(ILoginService loginService, ITokenManager tokenManager)
        {
            _loginService = loginService;
            _tokenManager = tokenManager;
        }

        public async Task<HttpResponseMessage> VerifyCredentials(string username, string password)
        {
			string accessToken = await _tokenManager.GetAccessToken();
			return await _loginService.VerifyPlayerCredentials(username, password, accessToken);
        }

        public ClaimsPrincipal CreatePrincipal(PlayerModel player)
        {

            //Remembers the username that is logged in with. The ClaimTypes.Name constant specifies the
            //type of the claim (user's name), and username is the value of the claim, representing the logged-in user's username.
            var claims = new List<Claim> {
				new Claim("PlayerId", player.PlayerId.ToString()),
				new Claim("Username", player.Username),
				new Claim("InGameName", player.InGameName),
				new Claim("Elo", player.Elo.ToString()),
				new Claim("Currency", player.CurrencyAmount.ToString()),

			};

            //The ClaimsIdentity represents the identity of the user and contains
            //the claims associated with that identity.
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //The ClaimsPrincipal represents the security context of the user within the application,
            //encapsulating the user's identity and associated claims.
            //The method then returns this ClaimsPrincipal object, which can be used for authentication and authorization purposes within the application.
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

		public async Task<PlayerModel> GetPlayerFromResponse(HttpResponseMessage response)
		{
			var playerData = await response.Content.ReadAsStringAsync();
			var player = JsonSerializer.Deserialize<PlayerModel>(playerData);
			return player;
		}

		public async Task<bool> Logout(int playerId)
		{
			string accessToken = await _tokenManager.GetAccessToken();
			HttpResponseMessage response = await _loginService.LogoutAsync(playerId, accessToken);
			return response.IsSuccessStatusCode;
		}
	}
}
