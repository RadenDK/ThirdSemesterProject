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

        public bool VerifyLogin(string userName, string password)
        {
            string storedHashedPassword = _playerAccessor.GetPassword(userName);
            return BCrypt.Net.BCrypt.Verify(password, storedHashedPassword);
        }
    }
}
