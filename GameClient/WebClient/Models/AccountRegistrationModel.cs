using System.ComponentModel.DataAnnotations;

namespace WebClient.Models
{
	public class AccountRegistrationModel
	{
		[Required]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "The property value should be at least 5 characters long.")]
		public string Username { get; set; }

		[Required]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must be at least 8 characters, include at least one upper case letter, one lower case letter, and one numeric digit.")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Passwords are not equal")]
		public string ConfirmPassword { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "The property value should be at least 5 characters long.")]
		public string InGameName {  get; set; }

		[Required]
		public DateTime BirthDay { get; set; }
		
	}
}
