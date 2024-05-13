using GameClientApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;
using GameClientApi.DatabaseAccessors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Azure.Identity;

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


        [HttpPut("update")]
        public IActionResult UpdatePlayer([FromBody] PlayerModel player)
        {
            try
            {
                if (_playerLogic.UpdatePlayer(player))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new { message = "Player: " + player.Username + "was not successfully updated" });
                }
            }
            catch (ArgumentException ex)
            {
				return BadRequest(new { message = ex.Message });
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

        [HttpDelete("delete/{username}")]
        public IActionResult DeletePlayer(string username)
        {
            if (_playerLogic.DeletePlayer(username))
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { message = "Player: " + username + " was not deleted successfully" });
            }
        }

    }
}
