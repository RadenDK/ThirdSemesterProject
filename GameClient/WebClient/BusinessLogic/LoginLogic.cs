using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
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

        public ClaimsPrincipal CreatePrincipal(string username)
        {
			//Remembers the username that is logged in with. The ClaimTypes.Name constant specifies the
            //type of the claim (user's name), and username is the value of the claim, representing the logged-in user's username.
			var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

			//The ClaimsIdentity represents the identity of the user and contains
            //the claims associated with that identity.
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			//The ClaimsPrincipal represents the security context of the user within the application,
            //encapsulating the user's identity and associated claims.
            //The method then returns this ClaimsPrincipal object, which can be used for authentication and authorization purposes within the application.
			return new ClaimsPrincipal(identity);
        }

        //public void StorePlayerInSession(ISession session, PlayerModel player)
        //{
        //    session.SetString("Player", JsonSerializer.Serialize(player));
        //    session.SetString("InGameName", player.InGameName);
        //}
    }
}
