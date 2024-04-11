namespace WebClient.Models
{
    public class PlayerModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string InGameName { get; set; }
		public string Email { get; set; }
		public DateTime Birthday { get; set; }
		public int Elo { get; set; }
		public bool Banned { get; set; }
		public int CurrencyAmount { get; set; }

	}
}
