using WebClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameLobbyLogic
{
    Task<IEnumerable<GameLobbyModel>> GenerateRandomGameLobbies(int amountOfLobbies);
    Task<GameLobbyModel> GetGameLobbyById(int lobbyId);
}