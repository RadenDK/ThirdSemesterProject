namespace WebClient.Models
{
	public class GameLobbyModel
	{
        public int GameLobbyId { get; set; }
		public string LobbyName { get; set; }

        public int AmountOfPlayers { get; set; }

		public PlayerModel lobbyOwner { get; set; }
		public string Password { get; set; }
		public string InviteLink { get; set; }
		public LobbyChatModel LobbyChat { get; set; }

		public IEnumerable<PlayerModel> LobbyPlayers { get; set;}

		public bool IsPrivate()
		{
			return String.IsNullOrEmpty(Password);
		}
	}
}
