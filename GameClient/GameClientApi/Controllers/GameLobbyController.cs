using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using GameClientApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace GameClientApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
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
		[HttpGet("AllGameLobbies")]
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

		[HttpPost("join")]
		public IActionResult JoinGameLobby([FromBody] JoinGameLobbyRequest joinRequest)
		{
			try
			{
				GameLobbyModel gameLobby = _gameLobbyLogic.JoinGameLobby(joinRequest.PlayerId, joinRequest.GameLobbyId, joinRequest.LobbyPassword);

				return Ok(gameLobby);

			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (UnauthorizedAccessException ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
