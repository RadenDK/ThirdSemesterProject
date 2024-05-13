using WebClient.Models;

namespace WebClient.Services
{
	public interface ITokenService
	{
		Task<TokensModel> GetWebClientTokens(ApiAccountModel accountModel);
		Task<TokensModel> RefreshTokens(RefreshRequestModel refreshToken);
	}
}
