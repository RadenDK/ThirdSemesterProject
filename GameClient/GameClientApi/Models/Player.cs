namespace GameClientApi.Models
{
    public class Player
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string InGameName { get; set; }
        public string Elo { get; set; }
        public string Email { get; set; }
        public bool Banned { get; set; }
        public int CurrencyAmount { get; set; }
        public bool OnlineStatus { get; set; }

        public Player(string UserName, string Password, string InGameName, string Rank, string Email, bool Ban, int Currency, bool OnlineStatus)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.InGameName = InGameName;
            this.Elo = Rank;
            this.Email = Email;
            this.Banned = Banned;
            this.CurrencyAmount = CurrencyAmount;
            this.OnlineStatus = OnlineStatus;
        }

        public Player()
        {

        }

    }
}
