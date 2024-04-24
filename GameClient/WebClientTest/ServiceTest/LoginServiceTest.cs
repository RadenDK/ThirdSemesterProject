using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WebClient.Services;

namespace WebClientTest.ServiceTest
{
	public class LoginServiceTest
	{
		private readonly Mock<IHttpClientService> _mockHttpClientService;
		private readonly LoginService _loginService;

		public LoginServiceTest()
		{
			_mockHttpClientService = new Mock<IHttpClientService>();
			_loginService = new LoginService(_mockHttpClientService.Object);
		}

		[Fact]
		public async Task VerifyPlayerCredentials_CallsHttpClientService()
		{
			// Arrange
			var username = "testuser";
			var password = "testpassword";
			_mockHttpClientService.Setup(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()))
				.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

			// Act
			var result = await _loginService.VerifyPlayerCredentials(username, password);

			// Assert
			_mockHttpClientService.Verify(service => service.PostAsync(It.IsAny<string>(), It.IsAny<StringContent>()), Times.Once);
			Assert.True(result.IsSuccessStatusCode);
		}
	}

}
