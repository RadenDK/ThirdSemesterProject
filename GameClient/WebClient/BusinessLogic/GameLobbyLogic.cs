using Newtonsoft.Json;
using WebClient.Models;
using WebClient.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

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
}