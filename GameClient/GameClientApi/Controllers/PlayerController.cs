using GameClientApi.Services;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {

        private PlayerService _playerService;

        public PlayerController(IConfiguration configuration)
        {
            _playerService = new PlayerService(configuration);
        }

        [HttpPost("exists")]
        public IActionResult DoesPlayerExist([FromBody]Player player)
        {
            bool playerExists = _playerService.VerifyLogin(player.UserName, player.Password);
            if (playerExists)
            {
                return Ok(new {exists = true, message = "Success"});
            }
            else
            {
                return Unauthorized(new { exists = false, message = "Failure" });
            }

        }
    }
}
