using GameClientApi.Models;

namespace GameClientApi.Security
{
	public interface ISecurityHelper
	{
		TokenModel GenerateTokens(TokenRequestModel tokenRequest);

		TokenModel RefreshTokens(string refreshToken);
	}
}
