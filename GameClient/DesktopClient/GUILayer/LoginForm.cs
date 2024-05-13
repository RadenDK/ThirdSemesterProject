using DesktopClient.ControllerLayer;
using DesktopClient.GUILayer;

namespace DesktopClient
{
	public partial class LoginForm : Form
	{

		private AdminController _adminController;
		private ApplicationContextManager _applicationContextManager;
		public LoginForm(ApplicationContextManager applicationContextManager, AdminController adminController)
		{
			InitializeComponent();
			_adminController = adminController;
			_applicationContextManager = applicationContextManager;
			this.Resize += LoginForm_Resize;
		}


		private async void loginButton_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(textboxAdminId.Text, out int adminId))
			{
				MessageBox.Show("Enter valid admin id");
				return;
			}

			string password = maskedPasswordTextBox.Text;

			try
			{
				bool loginSuccessful = await _adminController.VerifyLogin(adminId, password);

				if (loginSuccessful)
				{
					_applicationContextManager.ShowAdminDashboardForm();
				}
				else
				{
					MessageBox.Show("Login Failed. Please check your credentials.");
				}
			} // Handles Exceptions from the VerifyLogin method
			catch (ArgumentException ex)
			{
				MessageBox.Show(ex.Message);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred: {ex.Message}");
			}
		}

		private void LoginForm_Resize(object sender, EventArgs e)
		{
			loginPanel.Location = new Point(
				this.ClientSize.Width / 2 - loginPanel.Size.Width / 2,
				this.ClientSize.Height / 2 - loginPanel.Size.Height / 2);
		}
	}
}

