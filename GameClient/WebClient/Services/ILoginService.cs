using WebClient.Models;

namespace WebClient.Services
{
    public interface ILoginService
    {
        Task<HttpResponseMessage> VerifyPlayerCredentials(string username, string password, string accessToken);
        Task<HttpResponseMessage> LogoutAsync(int playerId, string accessToken);
    }
}
