using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace WebClient.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private Uri BaseAddress;

    public HttpClientService(HttpClient httpClient)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5198/")
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpClient.GetAsync(url);
        }

    }
}
