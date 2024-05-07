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


            
            ApplicationConfiguration.Initialize();

            ApplicationContextManager applicationContextManager = new ApplicationContextManager();

            Application.Run(applicationContextManager);
        }
    }
}