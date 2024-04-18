using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.Services
{
    public interface IRegistrationService
    {
        Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount);
    }
}
