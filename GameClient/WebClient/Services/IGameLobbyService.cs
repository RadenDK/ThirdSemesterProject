using WebClient.Models;
using System.Threading.Tasks;

namespace WebClient.Services
{
    public interface IGameLobbyService
    {
        Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request, string accessToken);

        Task<List<GameLobbyModel>> GetAllGameLobbies(string accessToken);
        Task<GameLobbyModel> CreateGameLobby(GameLobbyModel newLobby, string username, string accessToken);
    }
}
