using WebClient.Models;
using System.Threading.Tasks;

namespace WebClient.Services
{
    public interface IGameLobbyService
    {
        Task<GameLobbyModel> GetGameLobbyById(int lobbyId);

        Task<List<GameLobbyModel>> GetAllGameLobbies();
        Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username);
    }
}
