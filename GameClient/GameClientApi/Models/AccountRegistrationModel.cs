
namespace GameClientApi.Models
{
	public class AccountRegistrationModel
	{
		public string Username { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public string InGameName { get; set; }

		public DateTime BirthDay { get; set; }
	}
}
