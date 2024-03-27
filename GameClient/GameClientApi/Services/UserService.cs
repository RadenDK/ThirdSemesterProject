using GameClientApi.DatabaseAccessors;
using GameClientApi.Models;

namespace GameClientApi.Services
{
    public class UserService
    {
        UserDatabaseAccessor _userAccessor;

        public UserService(IConfiguration configuration)
        {
            _userAccessor = new UserDatabaseAccessor(configuration);
        }

        public IEnumerable<User> GetUsers()
        {
            return _userAccessor.GetUsers();
        }
    }
}
