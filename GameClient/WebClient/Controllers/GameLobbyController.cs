using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebClient.Controllers
{
    public class GameLobbyController : Controller
    {
        private readonly IGameLobbyLogic _gameLobbyLogic;

        public GameLobbyController(IGameLobbyLogic gameLobbyLogic)
        {
            _gameLobbyLogic = gameLobbyLogic;
        }

		[HttpGet("CreateLobby")]
		public IActionResult CreateLobby()
		{
			return View();
		}

        [HttpGet("GameLobby")]
        public IActionResult GameLobby()
        {
            return View();
        }

		[HttpGet]
        public async Task<IActionResult> ViewAllGameLobbies()
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

        [HttpPost]
        public async Task<IActionResult> CreateGameLobby(GameLobbyModel newLobby)
        {
            if (ModelState.IsValid)
            {
                var userPrincipal = HttpContext.User;
                string username = _gameLobbyLogic.GetUsername(userPrincipal);
				GameLobbyModel gameLobby = await _gameLobbyLogic.CreateGameLobby(newLobby, username);
                return RedirectToAction("GameLobby", "GameLobby");
            }
            return View("CreateLobby");
        }
    }
}






