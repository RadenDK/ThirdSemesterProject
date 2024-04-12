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
    public class LoginControllerTests
    {
        [Fact]
        public async Task LoginToProfile_ReturnsRedirectToActionResult_WhenResponseIsSuccessStatusCode()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK });

            var controller = new LoginController(mockHttpClientService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
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
