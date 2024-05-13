using Newtonsoft.Json;

namespace WebClient.Models
{
	public class RefreshRequestModel
	{
		[JsonProperty("refreshToken")]
		public string RefreshToken { get; set; }
	}
}
