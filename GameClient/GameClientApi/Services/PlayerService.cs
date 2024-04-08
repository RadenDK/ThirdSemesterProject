using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using BC = BCrypt.Net.BCrypt;
namespace GameClientApi.Services
{
    public class PlayerService
    {
        IPlayerDatabaseAccessor _playerAccessor;

        public PlayerService(IConfiguration configuration, IPlayerDatabaseAccessor playerDatabaseAccessor)
        {
            _playerAccessor = new PlayerDatabaseAccessor(configuration);
            _playerAccessor = playerDatabaseAccessor;
        }

        public bool VerifyLogin(string userName, string password)
        {
            string storedHashedPassword = _playerAccessor.GetPassword(userName);
            return BC.Verify(password, storedHashedPassword);
        }
    }
}
