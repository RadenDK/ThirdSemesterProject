using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.BusinessLogic
{
    public class RegistrationLogic : IRegistrationLogic
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationLogic(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount)
        {
            return await _registrationService.SendAccountToApi(newAccount);
        }
    }
}
