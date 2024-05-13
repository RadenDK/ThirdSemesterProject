using GameClientApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;
using GameClientApi.DatabaseAccessors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PlayerController : Controller
    {

        private PlayerLogic _playerLogic;

        public PlayerController(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor)
        {
            _playerLogic = new PlayerLogic(configuration, playerDatabaseAccessor);
        }


        [HttpPost("verify")]
        public IActionResult DoesPlayerExist(LoginModel loginModel)
        {
            try
            {

                bool playerExists = _playerLogic.VerifyLogin(loginModel.Username, loginModel.Password);
                if (playerExists)
                {
                    PlayerModel player = _playerLogic.GetPlayer(loginModel.Username);
                    return Ok(player);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
			catch (ArgumentException ex)
			{
				return Forbid();
			}
		}

        [HttpPost("create")]
        public IActionResult CreatePlayer(AccountRegistrationModel accountRegistration)
        {
            try
            {
                if (_playerLogic.CreatePlayer(accountRegistration))
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

        [HttpPost("ban")]
        public IActionResult BanPlayer([FromBody] string username)
        {
            try
            {
                if (_playerLogic.BanPlayer(username))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new { message = "Player: " + username + "was not banned successfully" });
                }

            }
            catch (ArgumentException e)
            {
                return BadRequest($"{e.Message}");
            }
        }

		[HttpPost("unban")]
		public IActionResult UnbanPlayer([FromBody] string username)
		{
			try
			{
				if (_playerLogic.UnbanPlayer(username))
				{
					return Ok();
				}
				else
				{
					return BadRequest(new { message = "Player: " + username + "was not unbanned successfully" });
				}

			}
			catch (ArgumentException e)
			{
				return BadRequest($"{e.Message}");
			}
		}

		[HttpGet("AllPlayers")]
        public IActionResult GetListOfPlayers()
        {
            try
            {
                List<PlayerModel> allPlayers = _playerLogic.GetAllPlayers();
                return Ok(allPlayers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
