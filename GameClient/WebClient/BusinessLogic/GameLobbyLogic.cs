using Newtonsoft.Json;
using WebClient.Models;
using WebClient.Services;

public class GameLobbyLogic
{
    private readonly IHttpClientService _httpClientService;

    public GameLobbyLogic(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IEnumerable<GameLobbyModel>> GenerateRandomGameLobbies(int amountOfLobbies)
    {
        HttpResponseMessage response = await _httpClientService.Post("api/gameLobbies", "");
        string content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content))
        {
            return new List<GameLobbyModel>();
        }
        List<GameLobbyModel> gameLobbies = JsonConvert.DeserializeObject<List<GameLobbyModel>>(content);
        // Apply business logic to the gameLobbies here
        return gameLobbies;
    }

    public async Task<GameLobbyModel> GetGameLobbyById(int lobbyId)
    {
        HttpResponseMessage response = await _httpClientService.Post($"api/gameLobbies/{lobbyId}", "");
        string content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        GameLobbyModel gameLobby = JsonConvert.DeserializeObject<GameLobbyModel>(content);
        // Apply business logic to the gameLobby here
        return gameLobby;
    }
}