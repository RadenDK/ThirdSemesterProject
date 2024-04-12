namespace WebClient.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return _httpClient.PostAsync(url, content);
        }
    }
}
