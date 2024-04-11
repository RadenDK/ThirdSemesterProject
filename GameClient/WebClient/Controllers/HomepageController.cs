using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using WebClient.Views;

namespace WebClient.Controllers;

public class HomepageController: Controller
{
    [HttpGet]
    public ActionResult Homepage()
    {
        return View();
    }
}