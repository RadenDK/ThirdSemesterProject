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
        public IActionResult DoesPlayerExist([FromBody]string userName, string password)
        {
            bool playerExists = _playerService.VerifyLogin(userName, password);
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
