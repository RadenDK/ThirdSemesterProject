using DesktopClient.ControllerLayer;
using DesktopClient.Security;
using DesktopClient.ServiceLayer;
using DesktopClient.Services;
using Microsoft.Extensions.Configuration;

namespace DesktopClient.GUILayer
{
	public class ApplicationContextManager : ApplicationContext
	{
		private Form _currentForm;
		private readonly HttpClient _httpClient;
		private readonly HttpClientService _httpClientService;
		private ITokenManager _tokenManager;

		public ApplicationContextManager(IConfiguration configuration)
		{
			_httpClient = new HttpClient();
			_httpClientService = new HttpClientService(_httpClient);
			_tokenManager = new TokenManager(configuration, new TokenService(_httpClientService));

			// This is the form that starts up when the program launches
			ShowForm(GetLoginForm());
		}

		public void ShowLoginForm()
		{
			ShowForm(GetLoginForm());
		}

		public void ShowAdminDashboardForm()
		{
			ShowForm(GetAdminForm());
		}

		public void ShowPlayerManagementForm()
		{
			ShowForm(GetPlayerManagementForm());
		}

		// Helper method in showing a form
		private void ShowForm(Form form)
		{
			// Closes the current form
			CloseCurrentForm();

			_currentForm = form;

			// Adds the event of Exiting the program so that if the exit button is pressed it closes down the whole program
			form.FormClosed += ExitProgram;

			form.Show();
		}

		// This is to exit the whole program
		private void ExitProgram(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void CloseCurrentForm()
		{
			if (_currentForm != null)
			{
				// Removes the event of closing down the program so that when switching between forms does not straight up shut down the whole program
				_currentForm.FormClosed -= ExitProgram;

				_currentForm.Close();

				_currentForm = null;
			}
		}

		private LoginForm GetLoginForm()
		{
			AdminService adminService = new AdminService(_httpClientService);
			AdminController adminController = new AdminController(adminService, _tokenManager);
			return new LoginForm(this, adminController);
		}

		private AdminDashboardForm GetAdminForm()
		{
			return new AdminDashboardForm(this);
		}

		private PlayerManagement GetPlayerManagementForm()
		{
			IPlayerService playerService = new PlayerService(_httpClientService);
			PlayerController playerController = new PlayerController(playerService, _tokenManager);
			return new PlayerManagement(this, playerController);
		}
	}
}
