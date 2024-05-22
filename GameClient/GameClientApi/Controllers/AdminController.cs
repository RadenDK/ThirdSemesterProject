using GameClientApi.BusinessLogic;
using GameClientApi.DatabaseAccessors;
using Microsoft.AspNetCore.Mvc;
using GameClientApi.Models;

namespace GameClientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private AdminLogic _adminLogic;

        public AdminController(IAdminDatabaseAccessor adminDatabaseAccessor)
        {
            _adminLogic = new AdminLogic(adminDatabaseAccessor);
        }

        [HttpPut("admins/login")]
        public IActionResult VerifyAdminLogin(AdminLoginModel adminLoginModel)
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