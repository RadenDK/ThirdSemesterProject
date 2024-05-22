namespace WebClient.Services
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> PostAsync(string url, StringContent content);

        Task<HttpResponseMessage> PutAsync(string url, StringContent content);

        Task<HttpResponseMessage> GetAsync(string url);

        void SetAuthenticationHeader(string accessToken);
    }
}
