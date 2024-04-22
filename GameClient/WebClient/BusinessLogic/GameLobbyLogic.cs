using Newtonsoft.Json;
using WebClient.Models;
using WebClient.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;

public class GameLobbyLogic : IGameLobbyLogic
{
	private readonly IGameLobbyService _gameLobbyService;

	public GameLobbyLogic(IGameLobbyService gameLobbyService)
	{
		_gameLobbyService = gameLobbyService;
	}


	public async Task<List<GameLobbyModel>> GetAllGameLobbies()
	{
		List<GameLobbyModel> allGameLobbies = new List<GameLobbyModel>();
		try
		{
			allGameLobbies = await _gameLobbyService.GetAllGameLobbies();
		} catch (Exception ex)
		{
			// I dont know yet if the business logic should handle what happens when the API cant be called
		}

		return allGameLobbies;
	}

	public async Task<GameLobbyModel> GetGameLobbyById(int lobbyId)
	{
		return await _gameLobbyService.GetGameLobbyById(lobbyId);
	}

	public async Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username)
	{
		return await _gameLobbyService.CreateGameLobby(newLobby, username);
	}

	//Dette skal nok tilføjes til en Player klasse
	public string GetUsername(ClaimsPrincipal userPrincipal)
	{
        var usernameClaim = userPrincipal.FindFirst("Username");
        return usernameClaim?.Value;
    }
}