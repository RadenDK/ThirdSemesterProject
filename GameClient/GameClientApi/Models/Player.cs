namespace GameClientApi.Models
{
    public class Player
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string InGameName { get; set; }
        public string Rank { get; set; }
        public string Email { get; set; }
        public bool Ban { get; set; }
        public int Currency { get; set; }

        public Player(string UserName, string Password, string InGameName, string Rank, string Email, bool Ban, int Currency)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.InGameName = InGameName;
            this.Rank = Rank;
            this.Email = Email;
            this.Ban = Ban;
            this.Currency = Currency;
        }

        public Player()
        {

        }

    }
}
