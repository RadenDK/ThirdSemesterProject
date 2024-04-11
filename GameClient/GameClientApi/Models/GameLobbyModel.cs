namespace GameClientApi.Models
{
    public class GameLobbyModel
    {
        public int GameLobbyId { get; set; }
        public string LobbyName { get; set; }
        public int AmountOfPlayers { get; set; }
        public string Password { get; set; }
        public string InviveLink { get; set; }
        public int LobbyChatId { get; set; }

    }
}
