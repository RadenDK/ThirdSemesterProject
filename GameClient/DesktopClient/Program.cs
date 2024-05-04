using DesktopClient.ControllerLayer;
using DesktopClient.ServiceLayer;
using DesktopClient.Services;

namespace DesktopClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Create services required by AdminController
            IAdminService adminService = new AdminService(new HttpClientService());

            // Create an instance of AdminController
            AdminController adminController = new AdminController(adminService);

            
            ApplicationConfiguration.Initialize();


            Application.Run(new LoginForm(adminController));
        }
    }
}