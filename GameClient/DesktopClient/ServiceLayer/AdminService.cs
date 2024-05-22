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


        public async Task<bool> VerifyAdminLoginCredentials(int adminId, string password, string accesToken)
        {
            StringContent content = new StringContent(JsonSerializer.Serialize(new { AdminId = adminId, PasswordHash = password }), Encoding.UTF8, "application/json");
            
            _httpClientService.SetAuthenticationHeader(accesToken);

            HttpResponseMessage response = await _httpClientService.PutAsync("Admin/admins/login", content);

            return response.IsSuccessStatusCode;

        }
    }
}
