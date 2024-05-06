using GameClientApi.Models;

namespace GameClientApi.Security
{
	public interface ISecurityHelper
	{
		TokenModel GenerateWebClientTokens(TokenRequestModel tokenRequest);

		TokenModel RefreshTokens(string refreshToken);
	}
}
