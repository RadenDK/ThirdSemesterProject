namespace GameClientApi.Models
{
	public class GameLobbyModel
	{
		public int? GameLobbyId { get; set; }
		public string LobbyName { get; set; }
		public int AmountOfPlayers { get; set; }
		public string Password { get; set; }
		public string InviteLink { get; set; }
		public LobbyChatModel LobbyChat { get; set; }
		public PlayerModel Owner { get; set; }
		public List<PlayerModel> PlayersInLobby { get; set; }
	}
}
