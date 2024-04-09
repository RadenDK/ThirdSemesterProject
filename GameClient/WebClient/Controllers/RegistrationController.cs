using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Text;
using WebClient.Models;

namespace WebClient.Controllers
{
	public class RegistrationController : Controller
	{
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

				// Read the response from the API
				var responseContent = await response.Content.ReadAsStringAsync();

				// Pass the response content to a view
				return View("ApiResult", responseContent);
			} else
			{
				return View("Registration", newAccount);
			}

			
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
				var response = await client.PostAsync("Player/create", data);

				return response;
			}
		}


	}
}
