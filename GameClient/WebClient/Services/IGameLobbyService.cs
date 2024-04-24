using WebClient.Models;
using System.Threading.Tasks;

namespace WebClient.Services
{
    public interface IGameLobbyService
    {
        Task<GameLobbyModel> JoinGameLobby(JoinGameLobbyRequest request);

        Task<List<GameLobbyModel>> GetAllGameLobbies();
    }
}
