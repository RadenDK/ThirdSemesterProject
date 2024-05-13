using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Models;
using WebClient.Security;
using WebClient.Services;

namespace WebClient.BusinessLogic
{
    public class RegistrationLogic : IRegistrationLogic
    {
        private readonly IRegistrationService _registrationService;
        private ITokenManager _tokenManager;

        public RegistrationLogic(IRegistrationService registrationService, ITokenManager tokenManager)
        {
            _registrationService = registrationService;
            _tokenManager = tokenManager;
        }

        public async Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount)
        {
			string accessToken = await _tokenManager.GetAccessToken();
			return await _registrationService.SendAccountToApi(newAccount, accessToken);
        }
    }
}
