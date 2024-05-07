using DesktopClient.ModelLayer;
using DesktopClient.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ServiceLayer
{
	public interface IPlayerService
	{
		Task<List<PlayerModel>> GetAllPlayers(string accessToken);

		Task<bool> BanPlayer(string username, string accessToken);

    }
}
