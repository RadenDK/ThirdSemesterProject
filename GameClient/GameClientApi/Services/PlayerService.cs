using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;

namespace GameClientApi.Services
{
    public class PlayerService
    {
        PlayerDatabaseAccessor _userAccessor;

        public PlayerService(IConfiguration configuration)
        {
            _userAccessor = new PlayerDatabaseAccessor(configuration);
        }

        public IEnumerable<Player> GetUsers()
        {
            return _userAccessor.GetUsers();
        }
    }
}
