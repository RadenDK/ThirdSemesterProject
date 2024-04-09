using GameClientApi.Services;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;
using GameClientApi.DatabaseAccessors;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : Controller
    {

        private PlayerService _playerService;

        public PlayerController(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor)
        {
            _playerService = new PlayerService(configuration, playerDatabaseAccessor);
        }

        [HttpPost("exists")]
        public IActionResult DoesPlayerExist([FromBody]Player player)
        {
            bool playerExists = _playerService.VerifyLogin(player.UserName, player.Password);
            if (playerExists)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("create")]
        public IActionResult CreatePlayer(AccountRegistrationModel accountRegistration)
        {
            return Ok(accountRegistration);
        }
	}
}
