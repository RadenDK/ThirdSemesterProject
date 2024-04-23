namespace WebClientTest.ControllerTest
{
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using WebClient.Controllers;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Security.Claims;
	using System.Net;
	using WebClient.BusinessLogic;
	using WebClient.Models;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;
	using Microsoft.AspNetCore.Mvc.Routing;

	public class LoginControllerTests
    {
		[Fact]
		public async Task LoginToProfile_ReturnsRedirectToActionResult_WhenResponseIsSuccessStatusCode()
		{
			// Arrange
			var mockLoginLogic = new Mock<ILoginLogic>();
			mockLoginLogic.Setup(logic => logic.VerifyCredentials(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

			var mockPlayer = new PlayerModel { 
				Username = "testuser", 
				InGameName = "testname", 
				Elo = 0, 
				Banned = false, 
				CurrencyAmount = 0, 
				OnlineStatus = false 
			};

			var mockClaims = new List<Claim> { 
				new Claim("Username", mockPlayer.Username),
                new Claim("InGameName", mockPlayer.InGameName),
                new Claim("Elo", mockPlayer.Elo.ToString()),
                new Claim("Banned", mockPlayer.Banned.ToString()),
                new Claim("CurrencyAmount", mockPlayer.CurrencyAmount.ToString()),
                new Claim("OnlineStatus", mockPlayer.OnlineStatus.ToString())
			};

			var mockIdentity = new ClaimsIdentity(mockClaims, "TestAuthType");
			var mockPrincipal = new ClaimsPrincipal(mockIdentity);

			mockLoginLogic.Setup(logic => logic.GetPlayerFromResponse(It.IsAny<HttpResponseMessage>()))
				.ReturnsAsync(mockPrincipal);

			var controller = new LoginController(mockLoginLogic.Object);

			var mockHttpContext = new Mock<HttpContext>();
			var mockAuthenticationManager = new Mock<IAuthenticationService>();
			mockAuthenticationManager
				.Setup(m => m.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
				.Returns(Task.CompletedTask);
			mockHttpContext
				.Setup(m => m.RequestServices.GetService(typeof(IAuthenticationService)))
				.Returns(mockAuthenticationManager.Object);

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

			controller.ControllerContext.HttpContext = mockHttpContext.Object;

			// Act
			var result = await controller.LoginToProfile("testuser", "testpassword");

			// Assert
			var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("HomePage", redirectToActionResult.ActionName);
			Assert.Equal("Homepage", redirectToActionResult.ControllerName);
		}

		[Fact]
		public async Task LoginToProfile_ReturnsViewResult_WhenResponseIsNotSuccessStatusCode()
		{
			// Arrange
			var mockLoginLogic = new Mock<ILoginLogic>();
			mockLoginLogic.Setup(logic => logic.VerifyCredentials(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });

			var controller = new LoginController(mockLoginLogic.Object);

			// Act
			var result = await controller.LoginToProfile("testuser", "testpassword");

			// Assert
			var viewResult = Assert.IsType<ViewResult>(result);
			Assert.Equal("Index", viewResult.ViewName);
		}
	}

}
