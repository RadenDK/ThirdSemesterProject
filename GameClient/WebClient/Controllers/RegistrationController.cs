using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using WebClient.Models;

namespace WebClient.Controllers
{
	public class RegistrationController : Controller
	{

		private readonly ILogger<RegistrationController> _logger;

		public RegistrationController(ILogger<RegistrationController> logger)
		{
			_logger = logger;
		}

		[HttpGet("Registration")]
		public IActionResult Registration()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> CreatePlayer(AccountRegistrationModel newAccount)
		{

			if (TryValidateModel(newAccount))
			{
				var response = await SendAccountToApi(newAccount);

				// Check if the request was successful
				if (response.IsSuccessStatusCode)
				{
					// Pass the success status to the view
					TempData["SuccessMessage"] = "Account created successfully!";
					return RedirectToAction("Index", "Login");
				}
				else if (response.StatusCode == HttpStatusCode.BadRequest)
				{
					// Read the response from the API
					var responseContent = await response.Content.ReadAsStringAsync();

					// Parse the JSON response
					var errorResponse = JObject.Parse(responseContent);

					// Get the error message
					var errorMessage = errorResponse["message"].ToString();

					// Check the error message
					if (errorMessage == "Username already exists")
					{
						ModelState.AddModelError("Username", "The username already exists. Please choose a different username.");
					}
					else if (errorMessage == "InGameName already exists")
					{
						ModelState.AddModelError("InGameName", "The in game name already exists. Please choose a different in game name.");
					}
				}
				else
				{
					TempData["ErrorMessage"] = "An error occurred while trying to create the account. Please try again later.";
				}
			}

			// If we got this far, something failed, redisplay form
			return View("Registration", newAccount);
		}


		private async Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount)
		{
			using (var client = new HttpClient())
			{
				// Set the URI of the API endpoint
				client.BaseAddress = new Uri("http://localhost:5198/");

				// Create a new AccountRegistrationApiModel and copy the data from newAccount
				var apiModel = new AccountRegistrationApiModel
				{
					Username = newAccount.Username,
					Password = newAccount.Password,
					Email = newAccount.Email,
					InGameName = newAccount.InGameName,
					BirthDay = newAccount.BirthDay
				};

				// Serialize our AccountRegistrationApiModel to a JSON string
				var json = JsonConvert.SerializeObject(apiModel);

				// StringContent object along with the MediaTypeHeaderValue
				var data = new StringContent(json, Encoding.UTF8, "application/json");

				// Post the data to the API
				try
				{
					var response = await client.PostAsync("Player/create", data);
					return response;
				}
				catch (HttpRequestException ex)
				{
					// Return a meaningful error message
					var errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError)
					{
						Content = new StringContent("An error occurred while trying to create the account. Please try again later.")
					};
					return errorResponse;
				}
			}
		}


	}
}
