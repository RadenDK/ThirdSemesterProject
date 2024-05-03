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
        public IActionResult VerifyAdminLogin(int adminId, string password)
        {
            try
            {
                bool adminExists = _adminLogic.VerifyLogin(adminId, password);
                if (adminExists)
                {
                    var admin = _adminLogic.GetAdmin(adminId);
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