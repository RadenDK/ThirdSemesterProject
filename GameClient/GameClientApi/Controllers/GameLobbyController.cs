﻿using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using GameClientApi.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        [HttpPost("CreateGameLobby")]
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

        [HttpPost("join")]
		public IActionResult JoinGameLobby([FromBody] JoinGameLobbyRequest joinRequest)
		{
			try
			{
				GameLobbyModel gameLobby = _gameLobbyLogic.JoinGameLobby(joinRequest.PlayerId, joinRequest.GameLobbyId, joinRequest.LobbyPassword);

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
	}
}
