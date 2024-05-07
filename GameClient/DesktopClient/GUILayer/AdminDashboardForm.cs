using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient.GUILayer
{
	public partial class AdminDashboardForm : Form
	{
		ApplicationContextManager _applicationContextManager;
		public AdminDashboardForm(ApplicationContextManager applicationContextManager)
		{
			InitializeComponent();
			_applicationContextManager = applicationContextManager;
		}

		private void AdminDashboard_Load(object sender, EventArgs e)
		{
        }

		private void logoutButton_Click(object sender, EventArgs e)
		{
			_applicationContextManager.ShowLoginForm();
		}
		private void administratePlayersButton_Click(object sender, EventArgs e)
		{
			_applicationContextManager.ShowPlayerManagementForm();
		}

		private void administrateLobbiesButton_Click(object sender, EventArgs e)
		{

		}
		private void administrateShopButton_Click(object sender, EventArgs e)
		{

		}

	}
}
