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
		Task<List<PlayerModel>> GetAllPlayers();

		Task<bool> BanPlayer(string username);

    }
}
