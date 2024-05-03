using GameClientApi.Models;

namespace GameClientApi.BusinessLogic
{
    public interface IAdminLogic
    {
        public bool VerifyLogin(int adminId, string password);

        public AdminLoginModel GetAdmin(int adminId);
    }
}
