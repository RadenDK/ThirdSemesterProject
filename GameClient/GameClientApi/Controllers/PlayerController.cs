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

        [HttpGet("exists/{username}")]
        public IActionResult Get(string userName)
        {
            bool playerExists = _playerService.GetUserName(userName);
            if (playerExists)
            {
                return Ok(new {exists = true});
            }
            else
            {
                return Ok(new { exists = false });
            }

        }
    }
}
