namespace DesktopClient.GUILayer
{
	partial class AdminDashboardForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			logoutButton = new Button();
			administratePlayersButton = new Button();
			administrateLobbiesButton = new Button();
			administrateStoreButton = new Button();
			dashboardPanel = new Panel();
			dashboardPanel.SuspendLayout();
			SuspendLayout();
			// 
			// logoutButton
			// 
			logoutButton.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
			logoutButton.Location = new Point(415, 385);
			logoutButton.Margin = new Padding(3, 2, 3, 2);
			logoutButton.Name = "logoutButton";
			logoutButton.Size = new Size(81, 25);
			logoutButton.TabIndex = 0;
			logoutButton.Text = "Logout";
			logoutButton.UseVisualStyleBackColor = true;
			logoutButton.Click += logoutButton_Click;
			// 
			// administratePlayersButton
			// 
			administratePlayersButton.Font = new Font("Segoe UI", 12F);
			administratePlayersButton.Location = new Point(363, 114);
			administratePlayersButton.Margin = new Padding(3, 2, 3, 2);
			administratePlayersButton.Name = "administratePlayersButton";
			administratePlayersButton.Size = new Size(186, 48);
			administratePlayersButton.TabIndex = 1;
			administratePlayersButton.Text = "Administrate Players";
			administratePlayersButton.UseVisualStyleBackColor = true;
			administratePlayersButton.Click += administratePlayersButton_Click;
			// 
			// administrateLobbiesButton
			// 
			administrateLobbiesButton.Font = new Font("Segoe UI", 12F);
			administrateLobbiesButton.Location = new Point(363, 200);
			administrateLobbiesButton.Margin = new Padding(3, 2, 3, 2);
			administrateLobbiesButton.Name = "administrateLobbiesButton";
			administrateLobbiesButton.Size = new Size(186, 48);
			administrateLobbiesButton.TabIndex = 2;
			administrateLobbiesButton.Text = "Administrate Lobbies";
			administrateLobbiesButton.UseVisualStyleBackColor = true;
			administrateLobbiesButton.Click += administrateShopButton_Click;
			// 
			// administrateStoreButton
			// 
			administrateStoreButton.Font = new Font("Segoe UI", 12F);
			administrateStoreButton.Location = new Point(363, 285);
			administrateStoreButton.Margin = new Padding(3, 2, 3, 2);
			administrateStoreButton.Name = "administrateStoreButton";
			administrateStoreButton.Size = new Size(186, 48);
			administrateStoreButton.TabIndex = 3;
			administrateStoreButton.Text = "Administrate Store";
			administrateStoreButton.UseVisualStyleBackColor = true;
			// 
			// dashboardPanel
			// 
			dashboardPanel.Anchor = AnchorStyles.None;
			dashboardPanel.Controls.Add(administratePlayersButton);
			dashboardPanel.Controls.Add(logoutButton);
			dashboardPanel.Controls.Add(administrateStoreButton);
			dashboardPanel.Controls.Add(administrateLobbiesButton);
			dashboardPanel.Location = new Point(12, 12);
			dashboardPanel.Name = "dashboardPanel";
			dashboardPanel.Size = new Size(911, 503);
			dashboardPanel.TabIndex = 4;
			// 
			// AdminDashboardForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(935, 527);
			Controls.Add(dashboardPanel);
			Margin = new Padding(3, 2, 3, 2);
			Name = "AdminDashboardForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "AdminDashboard";
			Load += AdminDashboard_Load;
			dashboardPanel.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private Button logoutButton;
		private Button administratePlayersButton;
		private Button administrateLobbiesButton;
		private Button administrateStoreButton;
		private Panel dashboardPanel;
	}
}