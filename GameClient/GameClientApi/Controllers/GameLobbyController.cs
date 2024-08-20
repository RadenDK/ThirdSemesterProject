using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using GameClientApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GameLobbyController : Controller
    {
        private readonly GameLobbyLogic _gameLobbyLogic;
        private readonly PlayerLogic _playerLogic;
        private readonly IHubContext<GameHub> _hubContext;

        public GameLobbyController(IConfiguration configuration,
                                   IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor,
                                   IPlayerDatabaseAccessor playerDatabaseAccessor,
                                   IHubContext<GameHub> hubContext)
        {
            _playerLogic = new PlayerLogic(configuration, playerDatabaseAccessor);
            _gameLobbyLogic = new GameLobbyLogic(configuration, gameLobbyDatabaseAccessor, _playerLogic);
            _hubContext = hubContext;
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

        [HttpPost("gameLobby")]
        public async Task<IActionResult> CreateGameLobby([FromBody] CreateGameLobbyModel data)
        {
            GameLobbyModel gameLobby = data.newLobby;
            string username = data.username;
            try
            {
                GameLobbyModel createdGameLobby = _gameLobbyLogic.CreateGameLobby(gameLobby, username);
                if (createdGameLobby.GameLobbyId.HasValue)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", "A new game lobby has been created.");
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

        [HttpPost("joinGameLobby")]
        public async Task<IActionResult> JoinGameLobby([FromBody] JoinGameLobbyRequestModel joinRequestModel)
        {
            try
            {
                GameLobbyModel gameLobby = _gameLobbyLogic.JoinGameLobby(joinRequestModel.PlayerId, joinRequestModel.GameLobbyId, joinRequestModel.LobbyPassword);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", "A player has joined the game lobby.");
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

        [HttpPost("leaveGameLobby")]
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