using WebClient.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WebClient.Services;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WebClientTests.ControllerTests
{
    public class LoginControllerTest
    {
        [Fact]
        public async Task LoginToProfile_ReturnsBlankView_WhenResponseIsSucces()
        {
            // Arrange
            var mockHttpClientService = new Mock<IHttpClientService>();
            mockHttpClientService.Setup(s => s.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var controller = new LoginController(mockHttpClientService.Object);

            // Act
            var result = await controller.LoginToProfile("username", "password");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Blank", viewResult.ViewName);
        }
    }
}
