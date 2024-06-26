﻿namespace GameClientApi.Models
{
	public class GameLobbyModel
	{
		public int? GameLobbyId { get; set; }
		public string LobbyName { get; set; }
		public int AmountOfPlayers { get; set; }


		public string? PasswordHash { get; set; }
		public string? InviteLink { get; set; }
		public LobbyChatModel? LobbyChat { get; set; }
		public List<PlayerModel>? PlayersInLobby { get; set; }

		public bool IsPrivate()
		{
			return !String.IsNullOrEmpty(PasswordHash);
		}

        public string? GetInGameNameOfLobbyOwner()
        {
            string inGameNameOfLobbyOwner = "Not Found";
            if (PlayersInLobby != null)
            {
                foreach (PlayerModel player in PlayersInLobby)
                {
                    if (player.IsOwner)
                    {
                        inGameNameOfLobbyOwner = player.InGameName;
                    }
                }
            }
            return inGameNameOfLobbyOwner;
        }
    }
}
