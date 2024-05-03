using GameClientApi.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
		private const string AllowedUsernameConfig = "AllowWebApp:Username";
		private const string AllowedPasswordConfig = "AllowWebApp:Password";
		private const int accessTokenExpirationTimeInHours = 1;
		private const int refreshTokenExpirationTimeInDays = 7;

		public SecurityHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		private TokenModel GenerateTokens(List<Claim> claims)
		{
			JwtSecurityToken accessToken = CreateJwtToken(claims, TimeSpan.FromHours(accessTokenExpirationTimeInHours));
			JwtSecurityToken refreshToken = CreateJwtToken(claims, TimeSpan.FromDays(refreshTokenExpirationTimeInDays));

			string accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
			string refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

			TokenModel tokens = new TokenModel { AccessToken = accessTokenString, RefreshToken = refreshTokenString};
			return tokens;
		}

		public TokenModel GenerateWebClientTokens(TokenRequestModel tokenRequest)
		{
			TokenModel tokens = null;
			if (isValidJWTUsernameAndPassword(tokenRequest.Username, tokenRequest.Password))
			{
				List<Claim> claims = new List<Claim>
				{
					new Claim(ClaimTypes.Role, RoleEnum.WebClient.ToString())
				};
				tokens = GenerateTokens(claims);
			}
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

		private bool isValidJWTUsernameAndPassword(string username, string password)
		{
			bool validUsernameAndPassword = false;

			if (!string.IsNullOrEmpty(username) || !string.IsNullOrEmpty(password))
			{
				string allowedUsername = _configuration[AllowedUsernameConfig];
				string allowedPassword = _configuration[AllowedPasswordConfig];

				validUsernameAndPassword = username.Equals(allowedUsername) && password.Equals(allowedPassword);
			}

			return validUsernameAndPassword;
		}

		public TokenModel RefreshTokens(string refreshToken)
		{
			ClaimsPrincipal claims = GetPrincipalFromExpiredToken(refreshToken);
			Claim roleClaim = claims.FindFirst(ClaimTypes.Role);

			if (roleClaim != null)
			{
				return null;
			}

			switch (roleClaim.Value)
			{
				case "WebClient":
					TokenModel webTokens = GenerateWebClientTokens(new TokenRequestModel
					{
						Username = _configuration[AllowedUsernameConfig],
						Password = _configuration[AllowedPasswordConfig],
						Role = roleClaim.Value
					});
					return webTokens;
				case "DesktopClient":
					return null;
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
