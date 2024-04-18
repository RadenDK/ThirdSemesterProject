namespace WebClient.Services
{
    public interface ILoginService
    {
        Task<HttpResponseMessage> VerifyPlayerCredentials(string username, string password);
    }
}
