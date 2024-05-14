namespace GameClientApi.Models
{
	public class JoinGameLobbyRequestModel
	{
		public int PlayerId { get; set; }

		public int GameLobbyId { get; set; }

		public string? LobbyPassword { get; set; }
	}
}
