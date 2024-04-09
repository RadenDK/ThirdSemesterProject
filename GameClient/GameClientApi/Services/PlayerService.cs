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
            _playerAccessor = playerDatabaseAccessor;
        }

        public bool VerifyLogin(string userName, string password)
        {
            string storedHashedPassword = _playerAccessor.GetPassword(userName);
            if (storedHashedPassword == null)
            {
                return false;
            }
            return BC.Verify(password, storedHashedPassword);
        }


        public bool CreatePlayer(AccountRegistrationModel player)
        {
            if (_playerAccessor.UsernameExists(player.Username))
            {
                throw new ArgumentException("Username already exists");
            }
            if (_playerAccessor.InGameNameExists(player.InGameName))
            {
                throw new ArgumentException("InGameName already exists");
            }

            player.Password = BC.HashPassword(player.Password);

            return _playerAccessor.CreatePlayer(player);
        }
    }
}
