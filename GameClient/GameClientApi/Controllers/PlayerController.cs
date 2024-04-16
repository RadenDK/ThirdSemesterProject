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
        public IActionResult DoesPlayerExist(LoginModel loginModel)
        {
            bool playerExists = _playerService.VerifyLogin(loginModel.Username, loginModel.Password);
            if (playerExists)
            {
                var player = _playerService.GetPlayer(loginModel.Username);
                return Ok(player);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("create")]
        public IActionResult CreatePlayer(AccountRegistrationModel accountRegistration)
        {
			try
			{
				if (_playerService.CreatePlayer(accountRegistration))
				{
					return Ok(); // Return Ok if the player was created successfully
				}
				else
				{
					return BadRequest(new { message = "Error creating the player" });
				}
			}
			catch (ArgumentException e)
			{
				// Return BadRequest with the exception message
				return BadRequest(new { message = e.Message });
			}
		}
    }
}
