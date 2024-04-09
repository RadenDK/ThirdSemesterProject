using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;

namespace WebClient.Controllers
{
    public class LoginController : Controller
    {


        // GET: LoginController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string userName, string password)
        {
           
            var response = await SendCredentialsToApi(userName, password);
            if (response.IsSuccessStatusCode)
            {
                // If the API returned a 200 status code, redirect to the new view
                return View("Blank");
            }
            else
            {
                // If the API returned a 400 status code, return the same view
                return View();
            }
        }

        private async Task<HttpResponseMessage> SendCredentialsToApi(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5198/");
                var requestData = new { UserName = userName, Password = password };
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("Player/exists", content);
                return response;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
