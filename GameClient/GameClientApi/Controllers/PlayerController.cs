using GameClientApi.Services;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;
using GameClientApi.DatabaseAccessors;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
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
                return RedirectToAction("Blank", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home", new {error=true});
            }

        }

        [HttpPost("create")]
        public IActionResult CreatePlayer(Player player)
        {
            throw new NotImplementedException();
        }
	}
}
