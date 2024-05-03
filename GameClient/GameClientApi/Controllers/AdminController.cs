using GameClientApi.BusinessLogic;
using GameClientApi.DatabaseAccessors;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private AdminLogic _adminLogic;

        public AdminController(IConfiguration configuration, IAdminDatabaseAccessor adminDatabaseAccessor)
        {
            _adminLogic = new AdminLogic(configuration, adminDatabaseAccessor);
        }

        [HttpPost("verify")]
        public IActionResult VerifyAdminLogin(AdminLoginModel adminLoginModel
        )
        {
            try
            {
                bool adminExists = _adminLogic.VerifyLogin(adminLoginModel.AdminId, adminLoginModel.PasswordHash);
                if (adminExists)
                {
                    var admin = _adminLogic.GetAdmin(adminLoginModel.AdminId);
                    return Ok(admin);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}