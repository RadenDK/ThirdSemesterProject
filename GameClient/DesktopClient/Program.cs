using DesktopClient.ControllerLayer;
using DesktopClient.ServiceLayer;
using DesktopClient.Services;
using DesktopClient.GUILayer;

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
            // HttpClient httpClient = new HttpClient();

            // Create HttpClientService required by AdminService
            // IHttpClientService httpClientService = new HttpClientService(httpClient);

            // Create services required by AdminController
            // IAdminService adminService = new AdminService(httpClientService);

            // Create an instance of AdminController
            // AdminController adminController = new AdminController(adminService);

            
            ApplicationConfiguration.Initialize();
            // Create httpClient required by HttpClientService
            HttpClient httpClient = new HttpClient();

            // Create HttpClientService required by PlayerService
            IHttpClientService httpClientService = new HttpClientService(httpClient);

            // Create services required by PlayerController
            IPlayerService playerService = new PlayerService(httpClientService);

            PlayerController playerController = new PlayerController(playerService);

            Application.Run(new PlayerManagement(playerController));
        }
    }
}