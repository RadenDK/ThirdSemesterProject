using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Security
{
	public class TokenManager : ITokenManager
	{
		private readonly IConfiguration _configuration;
		private readonly ITokenService _tokenService;
		private IHttpContextAccessor _httpContextAccessor;

		public TokenManager(IConfiguration configuration, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
		{
			_configuration = configuration;
			_tokenService = tokenService;
			_httpContextAccessor = httpContextAccessor;
		}
		
		public async Task<string> GetAccessToken()
		{
			if (string.IsNullOrEmpty(WebClientJwtContainer.AccessToken))
			{
				TokensModel newTokens = await GetNewTokens();
				UpdateStaticJwt(newTokens);
			}

			else if (TokenIsExpired(WebClientJwtContainer.AccessToken))
			{
				TokensModel newTokens = await _tokenService.RefreshTokens(WebClientJwtContainer.RefreshToken);
				if (string.IsNullOrEmpty(newTokens.AccessToken))
				{
					newTokens = await GetNewTokens();
				}
				UpdateStaticJwt(newTokens);
			}

			return WebClientJwtContainer.AccessToken;
		}

		private async Task<TokensModel> GetNewTokens()
		{
			ApiAccountModel accountModel = GetApiAccountCredentials();
			TokensModel newTokens = await _tokenService.GetWebClientTokens(accountModel);
			return newTokens;
		}

		private void UpdateStaticJwt(TokensModel newTokens)
		{
			WebClientJwtContainer.AccessToken = newTokens.AccessToken;
			WebClientJwtContainer.RefreshToken = newTokens.RefreshToken;
		}

		private ApiAccountModel GetApiAccountCredentials()
		{
			string username = _configuration["AllowWebApp:Username"];
			string password = _configuration["AllowWebApp:Password"];
			string role = _configuration["AllowWebApp:Role"];

			ApiAccountModel credentials = new ApiAccountModel { Username = username, Password = password, Role = role };
			return credentials;
		}

		private bool TokenIsExpired(string accessToken)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(accessToken);

			bool isExpired = jwtToken.ValidTo <= DateTime.UtcNow.AddMinutes(1);

			return isExpired;
		}
	}
}
