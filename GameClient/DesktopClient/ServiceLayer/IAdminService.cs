using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ServiceLayer
{
    public interface IAdminService
    {
        public Task<bool> VerifyAdminLoginCredentials(int adminId, string password, string accessToken);
    }
}
