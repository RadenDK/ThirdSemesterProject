
namespace DesktopClient.Security
{
    public interface ITokenManager
    {
        Task<string> GetAccessToken();

    }
}

