using DesktopClient.ModelLayer;
using DesktopClient.ServiceLayer;
using DesktopClient.Security;
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
		private ITokenManager _tokenManager;
		public PlayerController(IPlayerService playerService, ITokenManager tokenManager)
		{
			_playerService = playerService;
			_tokenManager = tokenManager;
		}

		public async Task<List<PlayerModel>> GetAllPlayers()
		{
			string accessToken = await _tokenManager.GetAccessToken();
			List<PlayerModel> allPlayers = await _playerService.GetAllPlayers(accessToken);
			return allPlayers;
		}

		public async Task<bool> UpdatePlayer(PlayerModel player)
		{
			string accessToken = await _tokenManager.GetAccessToken();
			return await _playerService.UpdatePlayer(player, accessToken);
		}

        public async Task<bool> DeletePlayer(PlayerModel player)
        {
            string accessToken = await _tokenManager.GetAccessToken();
            return await _playerService.DeletePlayer(player.PlayerId, accessToken);
        }

    }
}
