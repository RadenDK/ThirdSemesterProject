namespace GameClientApi.Models
{
    public class AdminLoginModel
    {
        public int AdminId { get; set; }
        public string? PasswordHash { get; set; }
    }
}