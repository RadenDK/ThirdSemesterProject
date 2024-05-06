using GameClientApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameClientApi.Security
{
    public class SecurityHelper : ISecurityHelper
    {
        private readonly IConfiguration _configuration;
        private const string SecretKeyConfig = "SECRET_KEY";
        private const string AllowedWebClientUsernameConfig = "AllowWebApp:Username";
        private const string AllowedWebClientPasswordConfig = "AllowWebApp:Password";
        private const string AllowedDesktopClientUsernameConfig = "AllowDesktopApp:Username";
        private const string AllowedDesktopClientPasswordConfig = "AllowDesktopApp:Password";
        private const int accessTokenExpirationTimeInHours = 1;
        private const int refreshTokenExpirationTimeInDays = 7;

        public SecurityHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenModel GenerateTokens(TokenRequestModel request)
        {
            TokenModel tokens = null;
            try
            {
                RoleEnum role = GetRoleEnum(request);

                if (isValidJWTUsernameAndPassword(request.Username, request.Password, role))
                {
                    tokens = GenerateApplicationTokens(role);
                }

            }
            catch (ArgumentException ex)
            {
                // Dunno what to have here
            }
            return tokens;
        }

        private RoleEnum GetRoleEnum(TokenRequestModel tokenRequestModel)
        {
            bool tryConversionSuccess = Enum.TryParse(tokenRequestModel.Role, out RoleEnum role);
            if (tryConversionSuccess)
            {
                return role;
            }
            else
            {
                throw new ArgumentException("Invalid role provided in the request");
            }

        }

        private TokenModel GenerateApplicationTokens(RoleEnum role)
        {
            TokenModel tokens = null;

            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, role.ToString())
                };
            tokens = GenerateTokens(claims);

            return tokens;
        }

        private bool isValidJWTUsernameAndPassword(string username, string password, RoleEnum role)
        {
            bool validUsernameAndPassword = false;
            

            switch (role)
            {
                case RoleEnum.WebClient:
					if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
					{
						string allowedUsername = _configuration[AllowedWebClientUsernameConfig];
						string allowedPassword = _configuration[AllowedWebClientPasswordConfig];

						validUsernameAndPassword = username.Equals(allowedUsername) && password.Equals(allowedPassword);
					}
					break;

                case RoleEnum.DesktopClient:
					if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
					{
						string allowedUsername = _configuration[AllowedDesktopClientUsernameConfig];
						string allowedPassword = _configuration[AllowedDesktopClientPasswordConfig];

						validUsernameAndPassword = username.Equals(allowedUsername) && password.Equals(allowedPassword);
					}
					break;
                default:
                    break;
            }



            return validUsernameAndPassword;
        }


        private TokenModel GenerateTokens(List<Claim> claims)
        {
            JwtSecurityToken accessToken = CreateJwtToken(claims, TimeSpan.FromHours(accessTokenExpirationTimeInHours));
            JwtSecurityToken refreshToken = CreateJwtToken(claims, TimeSpan.FromDays(refreshTokenExpirationTimeInDays));

            string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            string refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            TokenModel tokens = new TokenModel { AccessToken = accessTokenString, RefreshToken = refreshTokenString };
            return tokens;
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, TimeSpan expiresIn)
        {
            SymmetricSecurityKey signingKey = GetSecurityKey();
            if (signingKey == null)
            {
                return null;
            }

            SigningCredentials credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.Add(expiresIn),
                signingCredentials: credentials
            );
        }

        private SymmetricSecurityKey GetSecurityKey()
        {
            string secretKey = _configuration[SecretKeyConfig];
            if (string.IsNullOrEmpty(secretKey))
            {
                return null;
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }


        public TokenModel RefreshTokens(string refreshToken)
        {
            ClaimsPrincipal claims = GetPrincipalFromExpiredToken(refreshToken);
            Claim roleClaim = claims.FindFirst(ClaimTypes.Role);

            if (roleClaim == null)
            {
                return null;
            }

            switch (roleClaim.Value)
            {
                case "WebClient":
                    TokenModel webTokens = GenerateTokens(new TokenRequestModel
                    {
                        Username = _configuration[AllowedWebClientUsernameConfig],
                        Password = _configuration[AllowedWebClientPasswordConfig],
                        Role = roleClaim.Value
                    });
                    return webTokens;
                case "DesktopClient":
					TokenModel desktopTokens = GenerateTokens(new TokenRequestModel
					{
						Username = _configuration[AllowedDesktopClientUsernameConfig],
						Password = _configuration[AllowedDesktopClientPasswordConfig],
						Role = roleClaim.Value
					});
					return desktopTokens;
				default:
                    return null;
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            // Define the parameters for token validation
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // We don't validate the audience
                ValidateIssuer = false, // We don't validate the issuer
                ValidateIssuerSigningKey = true, // We do validate the signing key
                IssuerSigningKey = GetSecurityKey(), // The signing key is retrieved from a method
                ValidateLifetime = false // We don't validate the lifetime
            };

            // Create a handler for JWT tokens
            var tokenHandler = new JwtSecurityTokenHandler();

            // Declare a variable to hold the validated token
            SecurityToken validatedToken;

            // Validate the token and get the principal
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);

            // Cast the validated token to a JWT token
            JwtSecurityToken jwtToken = validatedToken as JwtSecurityToken;

            // Check if the token is null or if the algorithm used is not the expected one
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                // If the token is invalid, throw an exception
                throw new SecurityTokenException("Invalid token");
            }

            // If everything is fine, return the principal
            return principal;
        }
    }
}
