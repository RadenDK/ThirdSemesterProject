namespace DesktopClient.Services
{
	public interface IHttpClientService
	{
		Task<HttpResponseMessage> PostAsync(string url, StringContent content);

		Task<HttpResponseMessage> GetAsync(string url);

        Task<HttpResponseMessage> DeleteAsync(string requestUri);
		
		Task<HttpResponseMessage> PutAsync(string url, StringContent content);

		void SetAuthenticationHeader(string accessToken);
    }
}
