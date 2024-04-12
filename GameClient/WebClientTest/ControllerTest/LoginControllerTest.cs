namespace WebClientTest.ControllerTest
{
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using WebClient.Controllers;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authentication;
    using WebClient.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Mvc.Routing;

    public class LoginControllerTests
    {
        [Fact]
        public async Task LoginToProfile_ReturnsRedirectToActionResult_WhenResponseIsSuccessStatusCode()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(service => service.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var mockUrlHelperFactory = new Mock<IUrlHelperFactory>();
            mockUrlHelperFactory
                .Setup(factory => factory.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(Mock.Of<IUrlHelper>());

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IHttpClientService>(mockHttpClientService.Object)
                .AddSingleton<IAuthenticationService>(mockAuthenticationService.Object)
                .AddSingleton<IUrlHelperFactory>(mockUrlHelperFactory.Object)
                .BuildServiceProvider();

            var controller = new LoginController(mockHttpClientService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = serviceProvider
                    }
                }
            };

            // Act
            var result = await controller.LoginToProfile("testuser", "testpassword");

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Blank", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task LoginToProfile_ReturnsViewResult_WhenResponseIsNotSuccessStatusCode()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.BadRequest });

            var controller = new LoginController(mockHttpClientService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await controller.LoginToProfile("testuser", "testpassword");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Index", viewResult.ViewName);
        }
    }

}
