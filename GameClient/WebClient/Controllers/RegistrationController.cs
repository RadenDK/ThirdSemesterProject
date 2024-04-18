using Microsoft.AspNetCore.Mvc;
using WebClient.Models;
using WebClient.Services;
using WebClient.BusinessLogic;
using System.Net;
using Newtonsoft.Json.Linq;

namespace WebClient.Controllers
{
    public class RegistrationController : Controller
    {

        private readonly ILogger<RegistrationController> _logger;
		private readonly IRegistrationLogic _registrationLogic;

		public RegistrationController(ILogger<RegistrationController> logger, IRegistrationLogic registrationLogic)
        {
            _logger = logger;
			_registrationLogic = registrationLogic;
		}

        [HttpGet("Registration")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayer(AccountRegistrationModel newAccount)
        {
            if (!TryValidateModel(newAccount))
            {
                return View("Registration", newAccount);
            }

            var response = await _registrationLogic.SendAccountToApi(newAccount);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Account created successfully!";
                return RedirectToAction("Index", "Login");
            }
            else
            {
                HandleErrorResponse(response);
                return View("Registration", newAccount);
            }
        }

        private async void HandleErrorResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorMessage = await GetErrorMessageFromResponse(response);
                if (errorMessage == ApiErrorMessages.UsernameExistsCode)
                {
                    ModelState.AddModelError("Username", "The username already exists. Please choose a different username.");
                }
                else if (errorMessage == ApiErrorMessages.InGameNameExistsCode)
                {
                    ModelState.AddModelError("InGameName", "The in game name already exists. Please choose a different in game name.");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while trying to create the account. Please try again later.";
            }
        }

        private async Task<string> GetErrorMessageFromResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var errorResponse = JObject.Parse(responseContent);
            return errorResponse["message"].ToString();
        }
    }
}
