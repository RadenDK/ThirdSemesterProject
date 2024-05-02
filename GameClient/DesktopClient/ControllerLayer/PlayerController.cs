using DesktopClient.ModelLayer;
using DesktopClient.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ControllerLayer
{
	public class PlayerController
	{
		private readonly IPlayerService _playerService;
		public PlayerController(IPlayerService playerService)
		{
			_playerService = playerService;
		}

		public async Task<List<PlayerModel>> GetAllPlayers()
		{
			List<PlayerModel> allPlayers = new List<PlayerModel>();
			allPlayers = await _playerService.GetAllPlayers();
			return allPlayers;
		}
	}
}
