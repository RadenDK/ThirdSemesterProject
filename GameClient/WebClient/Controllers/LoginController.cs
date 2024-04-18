using System.Diagnostics;
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

using WebClient.BusinessLogic;

namespace WebClient.Controllers
{
	public class LoginController : Controller
	{
		private readonly ILoginLogic _loginLogic;

		public LoginController(ILoginLogic loginLogic)
		{
			_loginLogic = loginLogic;
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
				var response = await _loginLogic.VerifyCredentials(username, password);
				if (response.IsSuccessStatusCode)
				{
					var playerData = await response.Content.ReadAsStringAsync();
					var player = JsonSerializer.Deserialize<PlayerModel>(playerData);

					var principal = _loginLogic.CreatePrincipal(username);
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					//_loginService.StorePlayerInSession(HttpContext.Session, player);

					return RedirectToAction("HomePage", "Homepage");
				}
				else
				{
					return View("Index");
				}
			}
			catch
			{
				return View("Error");
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
