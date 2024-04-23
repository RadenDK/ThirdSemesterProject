
namespace GameClientApi.Models
{
	public class JoinGameLobbyRequest
	{
		public int PlayerId { get; set; }

        public int GameLobbyId { get; set; }

		public string Password { get; set; }
    }
}
