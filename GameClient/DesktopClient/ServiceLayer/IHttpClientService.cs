namespace DesktopClient.Services
{
	public interface IHttpClientService
	{
		Task<HttpResponseMessage> PostAsync(string url, StringContent content);

		Task<HttpResponseMessage> GetAsync(string url);
	}
}
