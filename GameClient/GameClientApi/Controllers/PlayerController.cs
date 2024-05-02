using GameClientApi.BusinessLogic;
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
                    var player = _playerLogic.GetPlayer(loginModel.Username);
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

        [HttpPatch("ban")]
        public IActionResult BanPlayer(int id)
        {
            try
            {
                if (_playerLogic.BanPlayer(id))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(new { message = "Player: " + id + "was not banned successfully" });
                }

            }catch(ArgumentException e)
            {
                return BadRequest($"{e.Message}");
            }
        }
    }
}
