using WebClient.Models;

namespace WebClient.Security
{
	public interface ITokenManager
	{
		Task<string> GetAccessToken();

	}
}
