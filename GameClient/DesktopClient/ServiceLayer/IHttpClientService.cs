namespace DesktopClient.Services
{
	public interface IHttpClientService
	{
		Task<HttpResponseMessage> Post(string url, string data);

		Task<HttpResponseMessage> PostAsync(string url, StringContent content);

		Task<HttpResponseMessage> GetAsync(string url);

		Task<HttpResponseMessage> PatchAsync(string url, StringContent content);
	}
}
