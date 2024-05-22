using Newtonsoft.Json;
using System.Text;
using WebClient.Models;

namespace WebClient.Services
{
	public class TokenService : ITokenService
	{
		private readonly IHttpClientService _httpClientService;

		public TokenService(IHttpClientService httpClientService)
		{
			_httpClientService = httpClientService;
		}

		public async Task<TokensModel> GetWebClientTokens(ApiAccountModel apiAccountModel)
		{
			TokensModel tokens = new TokensModel();

			string jsonContent = JsonConvert.SerializeObject(apiAccountModel);
			StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			try
			{
				HttpResponseMessage response = await _httpClientService.PostAsync("Token/tokens", stringContent);

				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();
					tokens = JsonConvert.DeserializeObject<TokensModel>(responseContent);
				}
			}
			catch (Exception ex)
			{

			}
			return tokens;
		}

		public async Task<TokensModel> RefreshTokens(RefreshRequestModel refreshToken)
		{
			TokensModel tokens = null;

			string jsonContent = JsonConvert.SerializeObject(refreshToken);

			StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await _httpClientService.PostAsync("Token/tokens/refresh", stringContent);
				if (response.IsSuccessStatusCode)
				{
					string responseContent = await response.Content.ReadAsStringAsync();
					tokens = JsonConvert.DeserializeObject<TokensModel>(responseContent);
				}
			}
			catch (Exception ex)
			{

			}
			return tokens;
		}
	}
}
