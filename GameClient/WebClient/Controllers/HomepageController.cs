using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using WebClient.Views;

namespace WebClient.Controllers;

public class HomepageController: Controller
{
    [Authorize]
    [HttpGet("Homepage")]
    public ActionResult Homepage()
    {
        var inGameName = HttpContext.Session.GetString("InGameName");

        // Pass the in-game name to the view
        ViewBag.InGameName = inGameName;
        // Return the HomePageTest (Homepage) view
        return View("~/Views/HomePage/Homepage.cshtml");
    }
}