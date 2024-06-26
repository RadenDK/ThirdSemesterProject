﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ModelLayer
{
	public class PlayerModel
	{
		public int? PlayerId { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string InGameName { get; set; }
		public int Elo { get; set; }
		public string Email { get; set; }
		public bool Banned { get; set; }
		public int CurrencyAmount { get; set; }
		public bool IsOwner { get; set; }
		public int? GameLobbyId { get; set; }
		public bool OnlineStatus { get; set; }
	}
}
