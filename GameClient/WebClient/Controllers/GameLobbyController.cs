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
            IEnumerable<GameLobbyModel> gameLobbies = await _gameLobbyLogic.GenerateRandomGameLobbies(5);
            return View(gameLobbies.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GameLobby(int lobbyId)
        {
            GameLobbyModel gameLobby = await _gameLobbyLogic.GetGameLobbyById(lobbyId);
            return View(gameLobby);
        }
    }
}