using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

public interface IGameLobbyLogic
{
    Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request);

    Task<List<GameLobbyModel>> GetAllGameLobbies();

    Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username);

    string GetUsername(ClaimsPrincipal userPrincipal);

    Task<bool> LeaveGameLobby(int playerId, int gameLobbyId);
}