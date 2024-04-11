namespace WebClient.Models
{
    public class PlayerModel
    {
        public string UserName { get; }
        public string Password { get; }
        public string InGameName { get; set; }
        public string Elo { get; set; }
        public string Email { get; set; }
        public bool Banned { get; set; }
        public int CurrencyAmount { get; set; }
        public bool OnlineStatus { get; set; }

    }
}
