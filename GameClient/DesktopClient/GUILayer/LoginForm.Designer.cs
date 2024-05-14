namespace DesktopClient
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			textboxAdminId = new TextBox();
			passwordLabel = new Label();
			userNameLabel = new Label();
			maskedPasswordTextBox = new MaskedTextBox();
			loginLabel = new Label();
			loginButton = new Button();
			loginPanel = new Panel();
			loginPanel.SuspendLayout();
			SuspendLayout();
			// 
			// textboxAdminId
			// 
			textboxAdminId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			textboxAdminId.Location = new Point(184, 101);
			textboxAdminId.Margin = new Padding(3, 2, 3, 2);
			textboxAdminId.Name = "textboxAdminId";
			textboxAdminId.Size = new Size(294, 23);
			textboxAdminId.TabIndex = 0;
			// 
			// passwordLabel
			// 
			passwordLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			passwordLabel.AutoSize = true;
			passwordLabel.Location = new Point(184, 148);
			passwordLabel.Name = "passwordLabel";
			passwordLabel.Size = new Size(60, 15);
			passwordLabel.TabIndex = 4;
			passwordLabel.Text = "Password:";
			// 
			// userNameLabel
			// 
			userNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			userNameLabel.AutoSize = true;
			userNameLabel.Location = new Point(184, 84);
			userNameLabel.Name = "userNameLabel";
			userNameLabel.Size = new Size(60, 15);
			userNameLabel.TabIndex = 3;
			userNameLabel.Text = "Admin ID:";
			// 
			// maskedPasswordTextBox
			// 
			maskedPasswordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			maskedPasswordTextBox.Location = new Point(184, 166);
			maskedPasswordTextBox.Name = "maskedPasswordTextBox";
			maskedPasswordTextBox.PasswordChar = '*';
			maskedPasswordTextBox.Size = new Size(294, 23);
			maskedPasswordTextBox.TabIndex = 1;
			// 
			// loginLabel
			// 
			loginLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			loginLabel.AutoSize = true;
			loginLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			loginLabel.Location = new Point(302, 39);
			loginLabel.Name = "loginLabel";
			loginLabel.Size = new Size(53, 21);
			loginLabel.TabIndex = 2;
			loginLabel.Text = "Login";
			// 
			// loginButton
			// 
			loginButton.Location = new Point(184, 217);
			loginButton.Name = "loginButton";
			loginButton.Size = new Size(75, 23);
			loginButton.TabIndex = 2;
			loginButton.Text = "Login";
			loginButton.UseVisualStyleBackColor = true;
			loginButton.Click += loginButton_Click;
			// 
			// loginPanel
			// 
			loginPanel.Anchor = AnchorStyles.None;
			loginPanel.Controls.Add(loginButton);
			loginPanel.Controls.Add(loginLabel);
			loginPanel.Controls.Add(userNameLabel);
			loginPanel.Controls.Add(passwordLabel);
			loginPanel.Controls.Add(maskedPasswordTextBox);
			loginPanel.Controls.Add(textboxAdminId);
			loginPanel.ForeColor = SystemColors.ControlText;
			loginPanel.Location = new Point(129, 106);
			loginPanel.Name = "loginPanel";
			loginPanel.Size = new Size(676, 314);
			loginPanel.TabIndex = 7;
			// 
			// LoginForm
			// 
			AcceptButton = loginButton;
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(935, 527);
			Controls.Add(loginPanel);
			Margin = new Padding(3, 2, 3, 2);
			Name = "LoginForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Login";
			loginPanel.ResumeLayout(false);
			loginPanel.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
		private TextBox textboxAdminId;
		private Label passwordLabel;
		private Label userNameLabel;
		private MaskedTextBox maskedPasswordTextBox;
		private Label loginLabel;
		private Button loginButton;
		private Panel loginPanel;
	}
}
