namespace GameClientApi.Models;

public class AdminLoginModel
{
        public int AdminId { get; }
        public string PasswordHash { get; set; }
}