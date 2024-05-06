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
			SuspendLayout();
			// 
			// logoutButton
			// 
			logoutButton.Font = new Font("Segoe UI", 11F);
			logoutButton.Location = new Point(12, 12);
			logoutButton.Name = "logoutButton";
			logoutButton.Size = new Size(93, 56);
			logoutButton.TabIndex = 0;
			logoutButton.Text = "Logout";
			logoutButton.UseVisualStyleBackColor = true;
			logoutButton.Click += logoutButton_Click;
			// 
			// administratePlayersButton
			// 
			administratePlayersButton.Font = new Font("Segoe UI", 12F);
			administratePlayersButton.Location = new Point(251, 80);
			administratePlayersButton.Name = "administratePlayersButton";
			administratePlayersButton.Size = new Size(213, 64);
			administratePlayersButton.TabIndex = 1;
			administratePlayersButton.Text = "Administrate Players";
			administratePlayersButton.UseVisualStyleBackColor = true;
			administratePlayersButton.Click += this.administratePlayersButton_Click;
			// 
			// administrateLobbiesButton
			// 
			administrateLobbiesButton.Font = new Font("Segoe UI", 12F);
			administrateLobbiesButton.Location = new Point(251, 160);
			administrateLobbiesButton.Name = "administrateLobbiesButton";
			administrateLobbiesButton.Size = new Size(213, 64);
			administrateLobbiesButton.TabIndex = 2;
			administrateLobbiesButton.Text = "Administrate Lobbies";
			administrateLobbiesButton.UseVisualStyleBackColor = true;
			administrateLobbiesButton.Click += this.administrateLobbiesButton_Click;
			// 
			// administrateStoreButton
			// 
			administrateStoreButton.Font = new Font("Segoe UI", 12F);
			administrateStoreButton.Location = new Point(251, 244);
			administrateStoreButton.Name = "administrateStoreButton";
			administrateStoreButton.Size = new Size(213, 64);
			administrateStoreButton.TabIndex = 3;
			administrateStoreButton.Text = "Administrate Store";
			administrateStoreButton.UseVisualStyleBackColor = true;
			administrateLobbiesButton.Click += this.administrateShopButton_Click;

			// 
			// AdminDashboardForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(administrateStoreButton);
			Controls.Add(administrateLobbiesButton);
			Controls.Add(administratePlayersButton);
			Controls.Add(logoutButton);
			Name = "AdminDashboardForm";
			Text = "AdminDashboard";
			Load += AdminDashboard_Load;
			ResumeLayout(false);
		}

		#endregion

		private Button logoutButton;
		private Button administratePlayersButton;
		private Button administrateLobbiesButton;
		private Button administrateStoreButton;
	}
}