using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;

        public LoginController()
        {
            _client = new HttpClient();
        }

        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string userName, string password)
        {
            var playerData = new { UserName = userName, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(playerData), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://localhost:7292/Player/exists", content);

            if (response.IsSuccessStatusCode)
            {
                // If the API returned a 200 status code, redirect to the new view
                return RedirectToAction("Blank");
            }
            else
            {
                // If the API returned a 400 status code, return the same view
                return View();
            }
        }
    }
}
