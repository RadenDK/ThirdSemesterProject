﻿namespace DesktopClient.Services
{
	public interface IHttpClientService
	{
		Task<HttpResponseMessage> PostAsync(string url, StringContent content);

		Task<HttpResponseMessage> GetAsync(string url);

        void SetAuthenticationHeader(string accessToken);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
    }
}
