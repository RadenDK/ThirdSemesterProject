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
using Humanizer;

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
				HttpResponseMessage response = await _loginLogic.VerifyCredentials(username, password);
				if (response.IsSuccessStatusCode)
				{
					PlayerModel player = await _loginLogic.GetPlayerFromResponse(response);
					ClaimsPrincipal principal = _loginLogic.CreatePrincipal(player);

					//Creates an encrypted authentication ticket(cookie) containing the user's principal (identity) information and adds it to the current response.
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("HomePage", "Homepage", player);
				}
				else
				{
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        ViewBag.ErrorMessage = "Username or password is incorrect.";
                    }
                    if (response.StatusCode == HttpStatusCode.Forbidden)
					{
						ViewBag.ErrorMessage = "You are banned dumbass🔥🔥";
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

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
