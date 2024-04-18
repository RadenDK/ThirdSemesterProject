using WebClient.Models;

namespace WebClient.Services
{
    public class GameLobbyService : IGameLobbyService
    {
        private readonly IHttpClientService _httpClientService;

        public GameLobbyService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public async Task<GameLobbyModel> GetGameLobbyById(int lobbyId)
        {
            return new GameLobbyModel();
        }
    }
}
