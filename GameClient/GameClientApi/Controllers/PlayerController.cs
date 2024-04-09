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

        [HttpPost("verify")]
        public IActionResult DoesPlayerExist([FromBody]string userName, string password)
        {
            bool playerExists = _playerService.VerifyLogin(userName, password);
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
