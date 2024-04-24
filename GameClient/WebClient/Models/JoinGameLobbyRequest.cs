namespace WebClient.Models
{
	public class JoinGameLobbyRequest
	{
		public int PlayerId {  get; set; }
		public int GameLobbyId { get; set; }
		public string? LobbyPassword { get; set; }

	}
}
