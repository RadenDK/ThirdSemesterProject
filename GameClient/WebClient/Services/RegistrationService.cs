using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using WebClient.Models;

namespace WebClient.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IHttpClientService _httpClientService;

        public RegistrationService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }
        public async Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount, string accessToken)
        {
            var apiModel = new AccountRegistrationModel
            {
                Username = newAccount.Username,
                Password = newAccount.Password,
                Email = newAccount.Email,
                InGameName = newAccount.InGameName,
                BirthDay = newAccount.BirthDay
            };

            var json = JsonConvert.SerializeObject(apiModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
				_httpClientService.SetAuthenticationHeader(accessToken);

				return await _httpClientService.PostAsync("Player/players", data);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred while trying to create the account. Please try again later.")
                };
            }
        }
    }
}
