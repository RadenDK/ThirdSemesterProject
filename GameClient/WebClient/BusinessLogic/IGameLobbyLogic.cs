using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameLobbyLogic
{
    Task<GameLobbyModel> GetGameLobbyById(int lobbyId);

    Task<List<GameLobbyModel>> GetAllGameLobbies();

    Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby);
}