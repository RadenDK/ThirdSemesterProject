using GameClientApi.Services;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {

        private PlayerService _userService;

        public PlayerController(IConfiguration configuration)
        {
            _userService = new PlayerService(configuration);
        }

        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return _userService.GetUsers();

        }
    }
}
