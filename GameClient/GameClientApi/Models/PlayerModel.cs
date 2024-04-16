namespace GameClientApi.Models
{
    public class PlayerModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string InGameName { get; set; }
        public string Rank { get; set; }
        public string Email { get; set; }
        public bool Ban { get; set; }
        public int Currency { get; set; }
        public bool IsOwner { get; set; }

        

    }
}
