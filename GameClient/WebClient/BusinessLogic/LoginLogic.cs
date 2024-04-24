using Microsoft.AspNetCore.Authentication.Cookies;
using System.Numerics;
using System.Security.Claims;
using System.Text.Json;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.BusinessLogic
{
    public class LoginLogic : ILoginLogic
    {
        private readonly ILoginService _loginService;

        public LoginLogic(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<HttpResponseMessage> VerifyCredentials(string username, string password)
        {
            return await _loginService.VerifyPlayerCredentials(username, password);
        }

        public ClaimsPrincipal CreatePrincipal(PlayerModel player)
        {

            //Remembers the username that is logged in with. The ClaimTypes.Name constant specifies the
            //type of the claim (user's name), and username is the value of the claim, representing the logged-in user's username.
            var claims = new List<Claim> {
                new Claim("Username", player.Username),
            };

            //The ClaimsIdentity represents the identity of the user and contains
            //the claims associated with that identity.
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //The ClaimsPrincipal represents the security context of the user within the application,
            //encapsulating the user's identity and associated claims.
            //The method then returns this ClaimsPrincipal object, which can be used for authentication and authorization purposes within the application.
            return new ClaimsPrincipal(identity);
        }

        public async Task<ClaimsPrincipal> GetPlayerFromResponse(HttpResponseMessage response)
        {
            var playerData = await response.Content.ReadAsStringAsync();
            var player = JsonSerializer.Deserialize<PlayerModel>(playerData);

            //Passes the username as a parameter to create a ClaimsPrincipal object representing the authenticated user's identity.
            var principal = CreatePrincipal(player);
            return principal;
        }
    }
}
