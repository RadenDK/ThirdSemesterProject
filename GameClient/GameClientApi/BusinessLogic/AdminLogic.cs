using GameClientApi.Models;
using GameClientApi.DatabaseAccessors;
using BC = BCrypt.Net.BCrypt;

namespace GameClientApi.BusinessLogic
{
    public class AdminLogic
    {
        IAdminDatabaseAccessor _adminAccessor;

        public AdminLogic(IConfiguration configuration, IAdminDatabaseAccessor adminDatabaseAccessor)
        {
            _adminAccessor = adminDatabaseAccessor;
        }

        public bool VerifyLogin(int adminId, string password)
        {
            string? storedHashedPassword = _adminAccessor.GetPassword(adminId);
            if (storedHashedPassword == null)
            {
                throw new ArgumentNullException("Stored HashedPassword is null");
            }
            return BC.Verify(password, storedHashedPassword);
        }

        public AdminModel GetAdmin(int adminId)
        {
            AdminModel adminData = _adminAccessor.GetAdmin(adminId);
            if (adminData == null)
            {
                throw new Exception("Admin not found");
            }
            return adminData;
        }
    }
}