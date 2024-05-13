using DesktopClient.Security;
using DesktopClient.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ControllerLayer
{
    public class AdminController
    {
        private readonly IAdminService _adminService;
        private ITokenManager _tokenManager;   

        public AdminController(IAdminService adminService, ITokenManager tokenMananger)
        {
            _adminService = adminService;
            _tokenManager = tokenMananger;
        }

        public async Task<bool> VerifyLogin(int adminId, string password)
        {
            //validation of admin id and password
            if (adminId <= 0 || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Invalid admin id or password.");
            }

            string accessToken = await _tokenManager.GetAccessToken(); 

            return await _adminService.VerifyAdminLoginCredentials(adminId, password, accessToken);
        }


    }
}
