using GameClientApi.Models;
using System.Collections.Generic;

namespace GameClientApi.Services
{
	public interface IGameLobbyService
	{
		IEnumerable<GameLobbyModel> GetAllGameLobbies();
	}
}
