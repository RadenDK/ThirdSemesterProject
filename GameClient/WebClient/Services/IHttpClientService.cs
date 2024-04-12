namespace WebClient.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}
