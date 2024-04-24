using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebClient.Controllers
{
    public class GameLobbyController : Controller
    {
        private readonly IGameLobbyLogic _gameLobbyLogic;

        public GameLobbyController(IGameLobbyLogic gameLobbyLogic)
        {
            _gameLobbyLogic = gameLobbyLogic;
        }

        [HttpGet]
        public IActionResult CreateLobby()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> JoinLobby()
        {
			IEnumerable<GameLobbyModel> gameLobbies = await _gameLobbyLogic.GetAllGameLobbies();

			return View(gameLobbies.ToList());
        }

		[HttpPost]
		public async Task<IActionResult> GameLobby([FromBody] JoinGameLobbyRequest request)
		{
            // TODO get the players id to pass along
            // request.playerId = PLAYERS ACTUALLY ID

            request.PlayerId = 1;

			GameLobbyModel gameLobby = await _gameLobbyLogic.JoinGameLobby(request);
			

			return View(gameLobby);
		}


	}
}