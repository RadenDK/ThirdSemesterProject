using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using GameClientApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameClientApi.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GameLobbyController : Controller
	{

		private GameLobbyService _gameLobbyService;
		private PlayerService _playerService;

		public GameLobbyController(IConfiguration configuration,
			IGameLobbyDatabaseAccessor gameLobbyDatabaseAccessor,
			IPlayerDatabaseAccessor playerDatabaseAccessor)
		{
			_playerService = new PlayerService(configuration, playerDatabaseAccessor);

			_gameLobbyService = new GameLobbyService(configuration,
				gameLobbyDatabaseAccessor, _playerService);
		}

		public IActionResult AllGameLobbies()
		{
			try
			{
				IEnumerable<GameLobbyModel> allGameLobbies = _gameLobbyService.GetAllGameLobbies();
				return Ok(allGameLobbies);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
