using GameClientApi.Models;
using GameClientApi.Security;
using Microsoft.AspNetCore.Mvc;

namespace GameClientApi.Controllers
{
	public class TokensController : Controller
	{
		private ISecurityHelper _securityHelper;

		public TokensController(ISecurityHelper securityHelper)
		{
			_securityHelper = securityHelper;
		}

		[Route("token")]
		[HttpPost]
		//Generate and return a JWT token
		public IActionResult GenerateTokens([FromBody] TokenRequestModel request)
		{
			TokenModel tokens = _securityHelper.GenerateTokens(request);

			if (!string.IsNullOrEmpty(tokens.AccessToken) && !string.IsNullOrEmpty(tokens.RefreshToken))
			{
				return Ok(tokens);
			}
			else
			{
				return BadRequest("Something went wrong");
			}
		}

		[HttpPost("token/refresh")]
		public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestModel refreshRequest)
		{
			TokenModel tokens = _securityHelper.RefreshTokens(refreshRequest.RefreshToken);

			if (!string.IsNullOrEmpty(tokens.AccessToken) && !string.IsNullOrEmpty(tokens.RefreshToken))
			{
				return Ok(tokens);
			}
			else
			{
				return BadRequest("Something went wrong");
			}
		}
	}
}
