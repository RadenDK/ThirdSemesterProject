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

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<bool> VerifyLogin(int adminId, string password)
        {
            return await _adminService.VerifyAdminLoginCredentials(adminId, password);
        }

        
    }
}
