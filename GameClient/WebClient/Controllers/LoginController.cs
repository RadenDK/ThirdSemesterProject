﻿using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net;

using WebClient.Services;

namespace WebClient.Controllers
{
	public class LoginController : Controller
	{
		private readonly IHttpClientService _httpClientService;

		public LoginController(IHttpClientService httpClientService)
		{
			_httpClientService = httpClientService;
		}

		// GET: LoginController
		public ActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
			return View();
		}


		[HttpPost]
		public async Task<ActionResult> LoginToProfile(string username, string password)
		{
			try
			{
				var response = await SendCredentialsToApi(username, password);
				if (response.IsSuccessStatusCode)
				{
					// Retrieve user information from the response or any other source
					var claims = new List<Claim> {
					new Claim(ClaimTypes.Name, username)
					};

					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					// Sign in the user
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					// Store player's data in the session
					var playerData = await response.Content.ReadAsStringAsync();
					var player = JsonSerializer.Deserialize<PlayerModel>(playerData);
					HttpContext.Session.SetString("Player", JsonSerializer.Serialize(player));
					HttpContext.Session.SetString("InGameName", player.InGameName);


					// If the API returned a 200 status code, redirect to the new view
					return RedirectToAction("HomePage", "Homepage");
				}
				else
				{
					if (response.StatusCode == HttpStatusCode.BadRequest)
					{
						ViewBag.ErrorMessage = "Username or password is incorrect.";
					}
					// If the API returned a 400 status code, return the same view
					return View("Index");
				}
			}
			catch
			{
				return View("Error");
			}
		}

		private async Task<HttpResponseMessage> SendCredentialsToApi(string username, string password)
		{
			var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { Username = username, Password = password }), System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClientService.PostAsync("Player/verify", content);
			return response;

		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
