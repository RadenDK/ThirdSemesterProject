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
            // Create httpClient required by HttpClientService
            HttpClient httpClient = new HttpClient();

            // Create HttpClientService required by AdminService
            IHttpClientService httpClientService = new HttpClientService(httpClient);

            // Create services required by AdminController
            IAdminService adminService = new AdminService(httpClientService);

            // Create an instance of AdminController
            AdminController adminController = new AdminController(adminService);

            
            ApplicationConfiguration.Initialize();


            Application.Run(new LoginForm(adminController));
        }
    }
}