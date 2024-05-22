using GameClientApi.Models;
using GameClientApi.Security;
using Microsoft.AspNetCore.Mvc;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : Controller
	{
		private ISecurityHelper _securityHelper;

		public TokenController(ISecurityHelper securityHelper)
		{
			_securityHelper = securityHelper;
		}

        [HttpPost("tokens")]
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

		[HttpPost("tokens/refresh")]
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
