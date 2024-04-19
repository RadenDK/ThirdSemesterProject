using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.BusinessLogic;
using WebClient.Models;
using WebClient.Views;

namespace WebClient.Controllers;

public class HomePageController: Controller
{
	private readonly IHomePageLogic _HomePageLogic;

	public HomePageController(IHomePageLogic homePageLogic)
	{
		_HomePageLogic = homePageLogic;
	}

	[Authorize]
    [HttpGet("Homepage")]
    public ActionResult Homepage()
    {
        // Get the user's in-game name from the claims
		var userPrincipal = HttpContext.User;
		var inGameNameClaim = _HomePageLogic.GetInGameName(userPrincipal);

        // Pass the in-game name to the view
        ViewBag.InGameName = inGameNameClaim;
        // Return the HomePageTest (Homepage) view
        return View("~/Views/HomePage/Homepage.cshtml");
    }
}