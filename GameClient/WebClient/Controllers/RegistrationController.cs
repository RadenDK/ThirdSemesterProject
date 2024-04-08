using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
	public class RegistrationController : Controller
	{
		public IActionResult Registration()
		{
			return View();
		}
	}
}
