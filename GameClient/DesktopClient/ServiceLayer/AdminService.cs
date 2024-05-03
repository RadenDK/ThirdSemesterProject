using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DesktopClient.ServiceLayer;

namespace DesktopClient.ServiceLayer
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5198/") };
        }
    }
}
