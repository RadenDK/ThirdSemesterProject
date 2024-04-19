using System.Security.Claims;

namespace WebClient.BusinessLogic;

public class HomePageLogic : IHomePageLogic
{
	public string GetInGameName(ClaimsPrincipal userPrincipal)
	{
		var inGameNameClaim = userPrincipal.FindFirst("InGameName");
		return inGameNameClaim?.Value;
	}
}