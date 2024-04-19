using System.Security.Claims;
using WebClient.Models;

namespace WebClient.BusinessLogic
{
    public interface ILoginLogic
    {
        Task<HttpResponseMessage> VerifyCredentials(string username, string password);
        ClaimsPrincipal CreatePrincipal(PlayerModel player);
        Task<ClaimsPrincipal> GetPlayerFromResponse(HttpResponseMessage response);
    }
}
