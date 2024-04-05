using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;

namespace GameClientApi.Services
{
    public class PlayerService
    {
        PlayerDatabaseAccessor _playerAccessor;

        public PlayerService(IConfiguration configuration)
        {
            _playerAccessor = new PlayerDatabaseAccessor(configuration);
        }

        public bool GetUserName(string userName)
        {
            return _playerAccessor.GetUserName(userName);
        }
    }
}
