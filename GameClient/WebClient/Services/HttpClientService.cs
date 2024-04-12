using System.Text;

namespace WebClient.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Post(string url, string data)
        {
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, StringContent content)
        {
            return await _httpClient.PostAsync(url, content);
        }
    }
}
