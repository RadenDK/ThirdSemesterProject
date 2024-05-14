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
    public ActionResult Homepage()
    {

        // Return the HomePage view
        return View("~/Views/HomePage/Homepage.cshtml");
    }
}