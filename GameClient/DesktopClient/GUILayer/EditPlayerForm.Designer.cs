namespace DesktopClient.GUILayer
{
	partial class EditPlayerForm
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
			playerManagementLabel = new Label();
			usernameLabel = new Label();
			inGameNameLabel = new Label();
			usernameTextBox = new TextBox();
			eloLabel = new Label();
			eloTextBox = new TextBox();
			emailLabel = new Label();
			currencyLabel = new Label();
			bannedLabel = new Label();
			bannedCheckBox = new CheckBox();
			confirmButton = new Button();
			inGameNameTextBox = new TextBox();
			emailTextBox = new TextBox();
			currencyTextBox = new TextBox();
			backButton = new Button();
			SuspendLayout();
			// 
			// playerManagementLabel
			// 
			playerManagementLabel.AutoSize = true;
			playerManagementLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			playerManagementLabel.Location = new Point(12, 9);
			playerManagementLabel.Name = "playerManagementLabel";
			playerManagementLabel.Size = new Size(202, 28);
			playerManagementLabel.TabIndex = 0;
			playerManagementLabel.Text = "Player Management";
			// 
			// usernameLabel
			// 
			usernameLabel.AutoSize = true;
			usernameLabel.Location = new Point(41, 134);
			usernameLabel.Name = "usernameLabel";
			usernameLabel.Size = new Size(78, 20);
			usernameLabel.TabIndex = 1;
			usernameLabel.Text = "Username:";
			// 
			// inGameNameLabel
			// 
			inGameNameLabel.AutoSize = true;
			inGameNameLabel.Location = new Point(391, 134);
			inGameNameLabel.Name = "inGameNameLabel";
			inGameNameLabel.Size = new Size(111, 20);
			inGameNameLabel.TabIndex = 2;
			inGameNameLabel.Text = "In Game Name:";
			// 
			// usernameTextBox
			// 
			usernameTextBox.Location = new Point(125, 127);
			usernameTextBox.Name = "usernameTextBox";
			usernameTextBox.Size = new Size(248, 27);
			usernameTextBox.TabIndex = 3;
			// 
			// eloLabel
			// 
			eloLabel.AutoSize = true;
			eloLabel.Location = new Point(86, 201);
			eloLabel.Name = "eloLabel";
			eloLabel.Size = new Size(33, 20);
			eloLabel.TabIndex = 5;
			eloLabel.Text = "Elo:";
			// 
			// eloTextBox
			// 
			eloTextBox.Location = new Point(125, 194);
			eloTextBox.Name = "eloTextBox";
			eloTextBox.Size = new Size(248, 27);
			eloTextBox.TabIndex = 6;
			// 
			// emailLabel
			// 
			emailLabel.AutoSize = true;
			emailLabel.Location = new Point(453, 201);
			emailLabel.Name = "emailLabel";
			emailLabel.Size = new Size(49, 20);
			emailLabel.TabIndex = 7;
			emailLabel.Text = "Email:";
			// 
			// currencyLabel
			// 
			currencyLabel.AutoSize = true;
			currencyLabel.Location = new Point(50, 263);
			currencyLabel.Name = "currencyLabel";
			currencyLabel.Size = new Size(69, 20);
			currencyLabel.TabIndex = 9;
			currencyLabel.Text = "Currency:";
			// 
			// bannedLabel
			// 
			bannedLabel.AutoSize = true;
			bannedLabel.Location = new Point(440, 263);
			bannedLabel.Name = "bannedLabel";
			bannedLabel.Size = new Size(62, 20);
			bannedLabel.TabIndex = 11;
			bannedLabel.Text = "Banned:";
			// 
			// bannedCheckBox
			// 
			bannedCheckBox.AutoSize = true;
			bannedCheckBox.Location = new Point(508, 266);
			bannedCheckBox.Name = "bannedCheckBox";
			bannedCheckBox.Size = new Size(18, 17);
			bannedCheckBox.TabIndex = 12;
			bannedCheckBox.UseVisualStyleBackColor = true;
			// 
			// confirmButton
			// 
			confirmButton.Location = new Point(693, 411);
			confirmButton.Name = "confirmButton";
			confirmButton.Size = new Size(100, 29);
			confirmButton.TabIndex = 13;
			confirmButton.Text = "Confirm";
			confirmButton.UseVisualStyleBackColor = true;
			confirmButton.Click += confirmButton_Click;
			// 
			// IngameNameTextBox
			// 
			inGameNameTextBox.Location = new Point(508, 127);
			inGameNameTextBox.Name = "IngameNameTextBox";
			inGameNameTextBox.Size = new Size(248, 27);
			inGameNameTextBox.TabIndex = 14;
			// 
			// emailTextBox
			// 
			emailTextBox.Location = new Point(508, 194);
			emailTextBox.Name = "emailTextBox";
			emailTextBox.Size = new Size(248, 27);
			emailTextBox.TabIndex = 15;
			// 
			// currencyTextBox
			// 
			currencyTextBox.Location = new Point(125, 256);
			currencyTextBox.Name = "currencyTextBox";
			currencyTextBox.Size = new Size(248, 27);
			currencyTextBox.TabIndex = 16;
			// 
			// backButton
			// 
			backButton.Location = new Point(587, 411);
			backButton.Name = "backButton";
			backButton.Size = new Size(100, 29);
			backButton.TabIndex = 17;
			backButton.Text = "Back";
			backButton.UseVisualStyleBackColor = true;
			backButton.Click += backButton_Click;
			// 
			// EditPlayerForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(backButton);
			Controls.Add(currencyTextBox);
			Controls.Add(emailTextBox);
			Controls.Add(inGameNameTextBox);
			Controls.Add(confirmButton);
			Controls.Add(bannedCheckBox);
			Controls.Add(bannedLabel);
			Controls.Add(currencyLabel);
			Controls.Add(emailLabel);
			Controls.Add(eloTextBox);
			Controls.Add(eloLabel);
			Controls.Add(usernameTextBox);
			Controls.Add(inGameNameLabel);
			Controls.Add(usernameLabel);
			Controls.Add(playerManagementLabel);
			Name = "EditPlayerForm";
			Text = "EditPlayerForm";
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label playerManagementLabel;
		private Label usernameLabel;
		private Label inGameNameLabel;
		private TextBox usernameTextBox;
		private Label eloLabel;
		private TextBox eloTextBox;
		private Label emailLabel;
		private Label currencyLabel;
		private Label bannedLabel;
		private CheckBox bannedCheckBox;
		private Button confirmButton;
		private TextBox inGameNameTextBox;
		private TextBox emailTextBox;
		private TextBox currencyTextBox;
		private Button backButton;
	}
}