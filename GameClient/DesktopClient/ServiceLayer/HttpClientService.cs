using System.Net.Http.Headers;

namespace DesktopClient.Services
{
	public class HttpClientService : IHttpClientService
	{
		private readonly HttpClient _httpClient;
		private Uri BaseAddress = new Uri("https://localhost:7092/api/");

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

		public async Task<HttpResponseMessage> PutAsync(string url, StringContent content)
		{
			return await _httpClient.PutAsync(url, content);
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
