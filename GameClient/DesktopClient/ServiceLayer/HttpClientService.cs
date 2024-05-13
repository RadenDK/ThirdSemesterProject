using DesktopClient.ServiceLayer;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DesktopClient.Services
{
	public class HttpClientService : IHttpClientService
	{
		private readonly HttpClient _httpClient;
		private Uri BaseAddress = new Uri("https://localhost:7092");

		public HttpClientService(HttpClient httpClient)
		{
			_httpClient = new HttpClient
			{
				BaseAddress = BaseAddress
			};

			_httpClient.DefaultRequestHeaders.Accept.Clear();
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<HttpResponseMessage> PostAsync(string url, StringContent content)
		{
			return await _httpClient.PostAsync(url, content);
		}

		public async Task<HttpResponseMessage> GetAsync(string url)
		{
			return await _httpClient.GetAsync(url);
		}

		public void SetAuthenticationHeader(string accessToken)
		{
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await _httpClient.DeleteAsync(requestUri);
        }
    }
}
