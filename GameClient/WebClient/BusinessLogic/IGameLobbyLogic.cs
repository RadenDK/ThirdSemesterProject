using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

public interface IGameLobbyLogic
{
    Task<GameLobbyModel> GetGameLobbyById(int lobbyId);

    Task<List<GameLobbyModel>> GetAllGameLobbies();

    Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username);

    string GetUsername(ClaimsPrincipal userPrincipal);
}