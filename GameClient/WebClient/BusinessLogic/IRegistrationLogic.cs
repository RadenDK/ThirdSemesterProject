using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Models;

namespace WebClient.BusinessLogic
{
    public interface IRegistrationLogic
    {
        Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount);
    }
}
