using Moq;
using Xunit;
using WebClient.Controllers;
using WebClient.Models;
using WebClient.Services;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebClientTest.ControllerTest
{
	public class RegistrationControllerTest
	{
		[Fact]
		public async Task CreatePlayer_ReturnsRedirectToActionResult_WhenResponseIsSuccessStatusCode()
		{
			// Arrange
			var mockHttpClientService = new Mock<IHttpClientService>();
			mockHttpClientService.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

			var mockUrlHelper = new Mock<IUrlHelper>();
			mockUrlHelper
				.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
				.Returns("callbackUrl");

			var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
			mockUrlHelperFactory
				.Setup(factory => factory.GetUrlHelper(It.IsAny<ActionContext>()))
				.Returns(mockUrlHelper.Object);

			var services = new ServiceCollection()
				.AddSingleton<IHttpClientService>(mockHttpClientService.Object)
				.AddSingleton<IUrlHelperFactory>(mockUrlHelperFactory.Object);

			var serviceProvider = services
				.AddMvc()
				.Services
				.BuildServiceProvider();

			var controller = new RegistrationController(new Mock<ILogger<RegistrationController>>().Object, mockHttpClientService.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = new DefaultHttpContext
					{
						RequestServices = serviceProvider
					}
				}
			};

			var newAccount = new AccountRegistrationModel
			{
				Username = "testuser",
				Password = "testPassword123",
				ConfirmPassword = "testPassword123",
				Email = "testemail@test.com",
				InGameName = "testname",
				BirthDay = new DateTime(1990, 1, 1)
			};

			// Act
			var result = await controller.CreatePlayer(newAccount);

			// Assert
			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectToActionResult.ActionName);
			Assert.Equal("Login", redirectToActionResult.ControllerName);
		}

		[Fact]
		public async Task CreatePlayer_ReturnsViewResult_WhenResponseIsNotSuccessStatusCode()
		{
			// Arrange
			var mockHttpClientService = new Mock<IHttpClientService>();
			mockHttpClientService.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest });

			var services = new ServiceCollection()
				.AddSingleton<IHttpClientService>(mockHttpClientService.Object);

			var serviceProvider = services
				.AddMvc()
				.Services
				.BuildServiceProvider();

			var controller = new RegistrationController(new Mock<ILogger<RegistrationController>>().Object, mockHttpClientService.Object)
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = new DefaultHttpContext
					{
						RequestServices = serviceProvider
					}
				}
			};

			var newAccount = new AccountRegistrationModel
			{
				Username = "testuser",
				Password = "testpassword123",
				ConfirmPassword = "testPassword123",
				Email = "testemail@test.com",
				InGameName = "testname",
				BirthDay = new DateTime(1990, 1, 1)
			};

			// Act
			var result = await controller.CreatePlayer(newAccount);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal("Registration", viewResult.ViewName);
		}
	}
}
