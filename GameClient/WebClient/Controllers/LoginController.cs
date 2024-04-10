﻿using System.Diagnostics;
using System.Linq.Expressions;
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

        public ActionResult Blank()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginToProfile(string username, string password)
        {
            var response = await SendCredentialsToApi(username, password);
            if (response.IsSuccessStatusCode)
            {
                // If the API returned a 200 status code, redirect to the new view
                return RedirectToAction("Blank");
            }
            else
            {
                // If the API returned a 400 status code, return the same view
                return View("Index");
            }
        }

        private static async Task<HttpResponseMessage> SendCredentialsToApi(string username, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5198/");
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { Username = username, Password = password }), System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("Player/verify", content);
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
