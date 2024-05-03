using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DesktopClient.ServiceLayer;
using DesktopClient.Services;

namespace DesktopClient.ServiceLayer
{
    public class AdminService : IAdminService
    {
        private readonly IHttpClientService _httpClientService;

        public AdminService(IHttpClientService httpClientService)
        {
            _httpClientService = httpClientService;
        }


        public async Task<HttpResponseMessage> VerifyAdminLoginCredentials(int adminId, string password)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { AdminId = adminId, PasswordHash = password }), Encoding.UTF8, "application/json");
            return await _httpClientService.PostAsync("Admin/verify", content);
        }
    }
}
