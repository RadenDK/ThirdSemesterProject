﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using WebClient.Models;
using WebClient.Services;

namespace WebClient.Controllers
{
    public class RegistrationController : Controller
    {

        private readonly ILogger<RegistrationController> _logger;
		private readonly IHttpClientService _httpClientService;

		public RegistrationController(ILogger<RegistrationController> logger, IHttpClientService httpClientService)
        {
            _logger = logger;
			_httpClientService = httpClientService;
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

            var response = await SendAccountToApi(newAccount);

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

        private async Task<HttpResponseMessage> SendAccountToApi(AccountRegistrationModel newAccount)
        {
            var apiModel = new AccountRegistrationApiModel
            {
                Username = newAccount.Username,
                Password = newAccount.Password,
                Email = newAccount.Email,
                InGameName = newAccount.InGameName,
                BirthDay = newAccount.BirthDay
            };

            var json = JsonConvert.SerializeObject(apiModel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                return await _httpClientService.PostAsync("Player/create", data);
            }
            catch (HttpRequestException)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("An error occurred while trying to create the account. Please try again later.")
                };
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
