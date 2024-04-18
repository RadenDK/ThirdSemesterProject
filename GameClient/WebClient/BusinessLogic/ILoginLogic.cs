using System.Security.Claims;

namespace WebClient.BusinessLogic
{
    public interface ILoginLogic
    {
        Task<HttpResponseMessage> VerifyCredentials(string username, string password);
        ClaimsPrincipal CreatePrincipal(string username);
        // void StorePlayerInSession(ISession session, PlayerModel player); // Uncomment this if you need it
    }
}
