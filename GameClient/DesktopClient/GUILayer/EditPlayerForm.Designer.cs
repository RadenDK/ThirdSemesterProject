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
			editPanel = new Panel();
			editPanel.SuspendLayout();
			SuspendLayout();
			// 
			// usernameLabel
			// 
			usernameLabel.AutoSize = true;
			usernameLabel.Location = new Point(24, 77);
			usernameLabel.Name = "usernameLabel";
			usernameLabel.Size = new Size(63, 15);
			usernameLabel.TabIndex = 1;
			usernameLabel.Text = "Username:";
			// 
			// inGameNameLabel
			// 
			inGameNameLabel.AutoSize = true;
			inGameNameLabel.Location = new Point(334, 77);
			inGameNameLabel.Name = "inGameNameLabel";
			inGameNameLabel.Size = new Size(89, 15);
			inGameNameLabel.TabIndex = 2;
			inGameNameLabel.Text = "In Game Name:";
			// 
			// usernameTextBox
			// 
			usernameTextBox.Location = new Point(97, 72);
			usernameTextBox.Margin = new Padding(3, 2, 3, 2);
			usernameTextBox.Name = "usernameTextBox";
			usernameTextBox.Size = new Size(218, 23);
			usernameTextBox.TabIndex = 3;
			// 
			// eloLabel
			// 
			eloLabel.AutoSize = true;
			eloLabel.Location = new Point(63, 128);
			eloLabel.Name = "eloLabel";
			eloLabel.Size = new Size(26, 15);
			eloLabel.TabIndex = 5;
			eloLabel.Text = "Elo:";
			// 
			// eloTextBox
			// 
			eloTextBox.Location = new Point(97, 123);
			eloTextBox.Margin = new Padding(3, 2, 3, 2);
			eloTextBox.Name = "eloTextBox";
			eloTextBox.Size = new Size(218, 23);
			eloTextBox.TabIndex = 6;
			// 
			// emailLabel
			// 
			emailLabel.AutoSize = true;
			emailLabel.Location = new Point(384, 128);
			emailLabel.Name = "emailLabel";
			emailLabel.Size = new Size(39, 15);
			emailLabel.TabIndex = 7;
			emailLabel.Text = "Email:";
			// 
			// currencyLabel
			// 
			currencyLabel.AutoSize = true;
			currencyLabel.Location = new Point(32, 174);
			currencyLabel.Name = "currencyLabel";
			currencyLabel.Size = new Size(58, 15);
			currencyLabel.TabIndex = 9;
			currencyLabel.Text = "Currency:";
			// 
			// bannedLabel
			// 
			bannedLabel.AutoSize = true;
			bannedLabel.Location = new Point(373, 174);
			bannedLabel.Name = "bannedLabel";
			bannedLabel.Size = new Size(50, 15);
			bannedLabel.TabIndex = 11;
			bannedLabel.Text = "Banned:";
			// 
			// bannedCheckBox
			// 
			bannedCheckBox.AutoSize = true;
			bannedCheckBox.Location = new Point(432, 175);
			bannedCheckBox.Margin = new Padding(3, 2, 3, 2);
			bannedCheckBox.Name = "bannedCheckBox";
			bannedCheckBox.Size = new Size(15, 14);
			bannedCheckBox.TabIndex = 12;
			bannedCheckBox.UseVisualStyleBackColor = true;
			// 
			// confirmButton
			// 
			confirmButton.Location = new Point(585, 290);
			confirmButton.Margin = new Padding(3, 2, 3, 2);
			confirmButton.Name = "confirmButton";
			confirmButton.Size = new Size(88, 22);
			confirmButton.TabIndex = 13;
			confirmButton.Text = "Confirm";
			confirmButton.UseVisualStyleBackColor = true;
			confirmButton.Click += confirmButton_Click;
			// 
			// inGameNameTextBox
			// 
			inGameNameTextBox.Location = new Point(432, 72);
			inGameNameTextBox.Margin = new Padding(3, 2, 3, 2);
			inGameNameTextBox.Name = "inGameNameTextBox";
			inGameNameTextBox.Size = new Size(218, 23);
			inGameNameTextBox.TabIndex = 14;
			// 
			// emailTextBox
			// 
			emailTextBox.Location = new Point(432, 123);
			emailTextBox.Margin = new Padding(3, 2, 3, 2);
			emailTextBox.Name = "emailTextBox";
			emailTextBox.Size = new Size(218, 23);
			emailTextBox.TabIndex = 15;
			// 
			// currencyTextBox
			// 
			currencyTextBox.Location = new Point(97, 169);
			currencyTextBox.Margin = new Padding(3, 2, 3, 2);
			currencyTextBox.Name = "currencyTextBox";
			currencyTextBox.Size = new Size(218, 23);
			currencyTextBox.TabIndex = 16;
			// 
			// backButton
			// 
			backButton.Location = new Point(491, 290);
			backButton.Margin = new Padding(3, 2, 3, 2);
			backButton.Name = "backButton";
			backButton.Size = new Size(88, 22);
			backButton.TabIndex = 17;
			backButton.Text = "Back";
			backButton.UseVisualStyleBackColor = true;
			backButton.Click += backButton_Click;
			// 
			// editPanel
			// 
			editPanel.Controls.Add(backButton);
			editPanel.Controls.Add(usernameLabel);
			editPanel.Controls.Add(currencyTextBox);
			editPanel.Controls.Add(inGameNameLabel);
			editPanel.Controls.Add(emailTextBox);
			editPanel.Controls.Add(usernameTextBox);
			editPanel.Controls.Add(inGameNameTextBox);
			editPanel.Controls.Add(eloLabel);
			editPanel.Controls.Add(confirmButton);
			editPanel.Controls.Add(eloTextBox);
			editPanel.Controls.Add(bannedCheckBox);
			editPanel.Controls.Add(emailLabel);
			editPanel.Controls.Add(bannedLabel);
			editPanel.Controls.Add(currencyLabel);
			editPanel.Anchor = AnchorStyles.None;
			editPanel.Location = new Point(12, 12);
			editPanel.Name = "editPanel";
			editPanel.Size = new Size(676, 314);
			editPanel.TabIndex = 18;
			// 
			// EditPlayerForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(700, 338);
			Controls.Add(editPanel);
			Margin = new Padding(3, 2, 3, 2);
			Name = "EditPlayerForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "EditPlayerForm";
			editPanel.ResumeLayout(false);
			editPanel.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
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
		private Panel editPanel;
	}
}