namespace WebClient.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> Post(string url, string data);
        Task<HttpResponseMessage> PostAsync(string url, StringContent content);
    }
}
