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
	}
}
