using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;
using BC = BCrypt.Net.BCrypt;
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
            return BC.Verify(password, storedHashedPassword);
        }
    }
}
