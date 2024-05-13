using Newtonsoft.Json;
using WebClient.Models;
using WebClient.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Security.Claims;
using WebClient.Security;

public class GameLobbyLogic : IGameLobbyLogic
{
	private readonly IGameLobbyService _gameLobbyService;
	private ITokenManager _tokenManager;

	public GameLobbyLogic(IGameLobbyService gameLobbyService, ITokenManager tokenManager)
	{
		_gameLobbyService = gameLobbyService;
		_tokenManager = tokenManager;
	}


	public async Task<List<GameLobbyModel>> GetAllGameLobbies()
	{
		List<GameLobbyModel> allGameLobbies = new List<GameLobbyModel>();
		try
		{
			string accessToken = await _tokenManager.GetAccessToken();
			allGameLobbies = await _gameLobbyService.GetAllGameLobbies(accessToken);
		} catch (Exception ex)
		{
			// I dont know yet if the business logic should handle what happens when the API cant be called
		}

		return allGameLobbies;
	}

	public async Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request)
	{
		string accessToken = await _tokenManager.GetAccessToken();
		return await _gameLobbyService.JoinGameLobby(request, accessToken);
	}

	public async Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username)
	{
		string accessToken = await _tokenManager.GetAccessToken();
		return await _gameLobbyService.CreateGameLobby(newLobby, username, accessToken);
	}

	//Dette skal nok tilføjes til en Player klasse
	public string GetUsername(ClaimsPrincipal userPrincipal)
	{
        var usernameClaim = userPrincipal.FindFirst("Username");
        return usernameClaim?.Value;
    }

	public async Task<bool> LeaveGameLobby(int playerId, int gameLobbyId)
	{
		string accessToken = await _tokenManager.GetAccessToken();
		return await _gameLobbyService.LeaveGameLobby(playerId, gameLobbyId, accessToken);
	}
}