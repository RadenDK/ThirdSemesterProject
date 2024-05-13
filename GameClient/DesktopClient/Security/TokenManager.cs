using Microsoft.Extensions.Configuration;
using DesktopClient.ServiceLayer;
using DesktopClient.ModelLayer;
using System.IdentityModel.Tokens.Jwt;

namespace DesktopClient.Security
{
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public TokenManager(IConfiguration configuration, ITokenService tokenService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<string> GetAccessToken()
        {
            if (string.IsNullOrEmpty(DesktopClientJwtContainer.AccessToken))
            {
                TokensModel newTokens = await GetNewTokens();
                UpdateStaticJwt(newTokens);
            }

            else if (TokenIsExpired(DesktopClientJwtContainer.AccessToken))
            {
                RefreshRequestModel refreshRequest = new RefreshRequestModel { RefreshToken = DesktopClientJwtContainer.RefreshToken };
                TokensModel newTokens = await _tokenService.RefreshTokens(refreshRequest);
                if (string.IsNullOrEmpty(newTokens.AccessToken))
                {
                    newTokens = await GetNewTokens();
                }
                UpdateStaticJwt(newTokens);
            }

            return DesktopClientJwtContainer.AccessToken;
        }

        private async Task<TokensModel> GetNewTokens()
        {
            ApiAccountModel accountModel = GetApiAccountCredentials();
            TokensModel newTokens = await _tokenService.GetDesktopClientTokens(accountModel);
            return newTokens;
        }

        private void UpdateStaticJwt(TokensModel newTokens)
        {
            DesktopClientJwtContainer.AccessToken = newTokens.AccessToken;
            DesktopClientJwtContainer.RefreshToken = newTokens.RefreshToken;
        }

        private ApiAccountModel GetApiAccountCredentials()
        {
            string username = _configuration["AllowDesktopApp:Username"];
            string password = _configuration["AllowDesktopApp:Password"];
            string role = _configuration["AllowDesktopApp:Role"];

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
