using GameClientApi.Services;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private UserService _userService;

        public UsersController(IConfiguration configuration)
        {
            _userService = new UserService(configuration);
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _userService.GetUsers();

        }
    }
}
