using DesktopClient.ControllerLayer;
using DesktopClient.ServiceLayer;
using DesktopClient.Services;
using DesktopClient.GUILayer;
using Microsoft.Extensions.Configuration;
using System.Collections;

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


			IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true);
			IConfigurationRoot configuration = builder.Build();


			ApplicationContextManager applicationContextManager = new ApplicationContextManager(configuration);

            Application.Run(applicationContextManager);
        }
    }
}