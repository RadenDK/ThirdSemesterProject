using System.Security.Claims;

namespace WebClient.BusinessLogic
{
    public interface IHomePageLogic
    {
        string GetInGameName(ClaimsPrincipal userPrincipal);
    }
}
