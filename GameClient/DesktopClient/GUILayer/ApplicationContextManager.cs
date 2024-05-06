using DesktopClient.ControllerLayer;
using DesktopClient.ServiceLayer;
using DesktopClient.Services;

namespace DesktopClient.GUILayer
{
	public class ApplicationContextManager : ApplicationContext
	{
		private Form _currentForm;

		public ApplicationContextManager()
		{
			ShowAdminDashboardForm();
		}

		public void ShowLoginForm()
		{
			CloseCurrentForm();

			LoginForm loginForm = GetLoginForm();
			_currentForm = loginForm;
			loginForm.Show();
		}

		public void ShowAdminDashboardForm()
		{
			CloseCurrentForm();

			AdminDashboardForm adminDashboardForm = GetAdminForm();
			_currentForm = adminDashboardForm;
			adminDashboardForm.Show();
		}

		public void ShowPlayerManagementForm()
		{
			CloseCurrentForm();

			PlayerManagement playerManagementForm = GetPlayerManagementForm();
			_currentForm = playerManagementForm;
			playerManagementForm.Show();
		}

		private void CloseCurrentForm()
		{
			if (_currentForm != null)
			{
				_currentForm.Close();
				_currentForm = null;
			}
		}

		private LoginForm GetLoginForm()
		{
			HttpClient httpClient = new HttpClient();
			HttpClientService httpClientService = new HttpClientService(httpClient);
			AdminService adminService = new AdminService(httpClientService);
			AdminController adminController = new AdminController(adminService);
			LoginForm loginForm = new LoginForm(this, adminController);

			return loginForm;
		}

		private AdminDashboardForm GetAdminForm()
		{
			AdminDashboardForm adminDashboardForm = new AdminDashboardForm(this);

			return adminDashboardForm;
		}

		private PlayerManagement GetPlayerManagementForm()
		{
			IPlayerService playerService = new PlayerService();
			PlayerController playerController = new PlayerController(playerService);
			PlayerManagement playerManagementForm = new PlayerManagement(this, playerController);

			return playerManagementForm;
		}

	}
}
