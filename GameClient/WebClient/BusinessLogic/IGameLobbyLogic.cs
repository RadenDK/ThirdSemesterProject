using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameLobbyLogic
{
    Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request);

    Task<List<GameLobbyModel>> GetAllGameLobbies();
}