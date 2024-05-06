using GameClientApi.Models;
using GameClientApi.DatabaseAccessors;
using BC = BCrypt.Net.BCrypt;

namespace GameClientApi.BusinessLogic
{
    public class AdminLogic : IAdminLogic
    {
        IAdminDatabaseAccessor _adminAccessor;

        public AdminLogic(IAdminDatabaseAccessor adminDatabaseAccessor)
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

        public AdminLoginModel GetAdmin(int adminId)
        {
            AdminLoginModel adminData = _adminAccessor.GetAdmin(adminId);
            if (adminData == null)
            {
                throw new Exception("Admin not found");
            }
            return adminData;
        }
    }
}