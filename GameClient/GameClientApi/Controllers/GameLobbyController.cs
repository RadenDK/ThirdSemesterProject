using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using GameClientApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace GameClientApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize]
	public class GameLobbyController : Controller
	{

		private GameLobbyLogic _gameLobbyLogic;
		private PlayerLogic _playerLogic;

		public GameLobbyController(IConfiguration configuration,
			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor,
			IPlayerDatabaseAccessor playerDatabaseAccessor)
		{
			_playerLogic = new PlayerLogic(configuration, playerDatabaseAccessor);

			_gameLobbyLogic = new GameLobbyLogic(configuration,
				gameLobbyDatabaseAccessor, _playerLogic);
		}

		[HttpGet("gameLobbies")]
		public IActionResult AllGameLobbies()
		{
			try
			{
				IEnumerable<GameLobbyModel> allGameLobbies = _gameLobbyLogic.GetAllGameLobbies();
				return Ok(allGameLobbies);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        [HttpPost("gameLobbies")]
        public IActionResult CreateGameLobby([FromBody] CreateGameLobbyModel data)
        {
            GameLobbyModel gameLobby = data.newLobby;
            string username = data.username;
            try
            {
                GameLobbyModel createdGameLobby = _gameLobbyLogic.CreateGameLobby(gameLobby, username);
                if (createdGameLobby.GameLobbyId.HasValue)
                {
                    return Ok(createdGameLobby);
                }
                else
                {
                    return BadRequest("Failed to create game lobby.");
                }
            }
            catch
            {
                return BadRequest("The wrong data was provided");
            }
        }

        [HttpPut("gamelobbies/join")]
		public IActionResult JoinGameLobby([FromBody] JoinGameLobbyRequestModel joinRequestModel)
		{
			try
			{
				GameLobbyModel gameLobby = _gameLobbyLogic.JoinGameLobby(joinRequestModel.PlayerId, joinRequestModel.GameLobbyId, joinRequestModel.LobbyPassword);

				return Ok(gameLobby);

			}
			catch (UnauthorizedAccessException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			
		}

        [HttpPut("gamelobbies/leave")]
        public IActionResult LeaveGameLobby([FromBody] LeaveGameLobbyRequestModel leaveRequest)
        {
            try
            {
                bool success = _gameLobbyLogic.LeaveGameLobby(leaveRequest);

                if (success)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Something went wrong with leaving lobby");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } 
	}
}
