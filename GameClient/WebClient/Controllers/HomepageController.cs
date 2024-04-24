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

	[Authorize]
    [HttpGet("Homepage")]
    public ActionResult Homepage(PlayerModel player)
    {
        // Pass the in-game name to the view
        ViewBag.InGameName = player.InGameName;
        // Return the HomePageTest (Homepage) view
        return View("~/Views/HomePage/Homepage.cshtml");
    }
}