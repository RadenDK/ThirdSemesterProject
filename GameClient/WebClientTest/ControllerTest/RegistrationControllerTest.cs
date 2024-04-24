using Moq;
using Xunit;
using WebClient.Controllers;
using WebClient.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using WebClient.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebClientTest.ControllerTest
{
	public class RegistrationControllerTest
	{
		private readonly Mock<IRegistrationLogic> _mockRegistrationLogic;
		private readonly RegistrationController _controller;

		public RegistrationControllerTest()
		{
			_mockRegistrationLogic = new Mock<IRegistrationLogic>();

			var mockLogger = new Mock<ILogger<RegistrationController>>();
			_controller = new RegistrationController(mockLogger.Object, _mockRegistrationLogic.Object);

			var mockHttpContext = new Mock<HttpContext>();
			_controller.ControllerContext.HttpContext = mockHttpContext.Object;

			var mockUrlHelper = new Mock<IUrlHelper>();
			mockUrlHelper
				.Setup(u => u.Action(It.IsAny<UrlActionContext>()))
				.Returns("callbackUrl");
			var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
			mockUrlHelperFactory
				.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
				.Returns(mockUrlHelper.Object);
			mockHttpContext
				.Setup(m => m.RequestServices.GetService(typeof(IUrlHelperFactory)))
				.Returns(mockUrlHelperFactory.Object);

			//Is used by TryValidateModel in controller. Is used to validate the model data in action methods.
			var mockValidator = new Mock<IObjectModelValidator>();
			mockValidator
				.Setup(v => v.Validate(It.IsAny<ActionContext>(), It.IsAny<ValidationStateDictionary>(), It.IsAny<string>(), It.IsAny<object>()));
			mockHttpContext
				.Setup(m => m.RequestServices.GetService(typeof(IObjectModelValidator)))
				.Returns(mockValidator.Object);

			//Is used by TempData in controller. Is used to store data that will be used in the next request.
			var mockTempData = new Mock<ITempDataDictionary>();
			var mockTempDataFactory = new Mock<ITempDataDictionaryFactory>();
			mockTempDataFactory.Setup(f => f.GetTempData(It.IsAny<HttpContext>())).Returns(mockTempData.Object);
			mockHttpContext
				.Setup(m => m.RequestServices.GetService(typeof(ITempDataDictionaryFactory)))
				.Returns(mockTempDataFactory.Object);
		}

		[Fact]
		public async Task CreatePlayer_ReturnsRedirectToActionResult_WhenResponseIsSuccessStatusCode()
		{
			// Arrange
			_mockRegistrationLogic.Setup(logic => logic.SendAccountToApi(It.IsAny<AccountRegistrationModel>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

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
			var result = await _controller.CreatePlayer(newAccount);

			// Assert
			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirectToActionResult.ActionName);
			Assert.Equal("Login", redirectToActionResult.ControllerName);
		}

		[Fact]
		public async Task CreatePlayer_ReturnsViewResult_WhenResponseIsNotSuccessStatusCode()
		{
			// Arrange
			_mockRegistrationLogic.Setup(logic => logic.SendAccountToApi(It.IsAny<AccountRegistrationModel>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

			var newAccount = new AccountRegistrationModel();

			// Act
			var result = await _controller.CreatePlayer(newAccount);

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal("Registration", viewResult.ViewName);
		}
	}

}
