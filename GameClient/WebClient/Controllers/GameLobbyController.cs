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
            //IEnumerable<GameLobbyModel> gameLobbies = await _gameLobbyLogic.GenerateRandomGameLobbies(5);
			IEnumerable<GameLobbyModel> gameLobbies = await _gameLobbyLogic.GetAllGameLobbies();

			return View(gameLobbies.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GameLobby(int lobbyId)
        {
            GameLobbyModel gameLobby = await _gameLobbyLogic.GetGameLobbyById(lobbyId);
            return View(gameLobby);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGameLobby(GameLobbyModel newLobby)
        {
            if(ModelState.IsValid) 
            {
                GameLobbyModel gameLobby = await _gameLobbyLogic.CreateGameLobby(newLobby);
                return RedirectToAction("GameLobby", new { lobbyId = gameLobby.GameLobbyId });
            }
            return View(newLobby);
        }
    }
}