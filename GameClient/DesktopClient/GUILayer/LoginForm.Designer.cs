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
            textboxUserName = new TextBox();
            loginLabel = new Label();
            userNameLabel = new Label();
            passwordLabel = new Label();
            loginButton = new Button();
            maskedPasswordTextBox = new MaskedTextBox();
            SuspendLayout();
            // 
            // textboxUserName
            // 
            textboxUserName.Location = new Point(256, 110);
            textboxUserName.Margin = new Padding(3, 2, 3, 2);
            textboxUserName.Name = "textboxUserName";
            textboxUserName.Size = new Size(240, 23);
            textboxUserName.TabIndex = 1;
            // 
            // loginLabel
            // 
            loginLabel.AutoSize = true;
            loginLabel.Location = new Point(256, 38);
            loginLabel.Name = "loginLabel";
            loginLabel.Size = new Size(37, 15);
            loginLabel.TabIndex = 2;
            loginLabel.Text = "Login";
            // 
            // userNameLabel
            // 
            userNameLabel.AutoSize = true;
            userNameLabel.Location = new Point(256, 93);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new Size(60, 15);
            userNameLabel.TabIndex = 3;
            userNameLabel.Text = "Admin ID:";
            userNameLabel.Click += userNameLabel_Click;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(256, 160);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(60, 15);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Password:";
            passwordLabel.Click += passwordLabel_Click;
            // 
            // loginButton
            // 
            loginButton.Location = new Point(256, 230);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(75, 23);
            loginButton.TabIndex = 5;
            loginButton.Text = "Login";
            loginButton.UseVisualStyleBackColor = true;
            loginButton.Click += loginButton_Click;
            // 
            // maskedPasswordTextBox
            // 
            maskedPasswordTextBox.Location = new Point(256, 182);
            maskedPasswordTextBox.Name = "maskedPasswordTextBox";
            maskedPasswordTextBox.Size = new Size(240, 23);
            maskedPasswordTextBox.TabIndex = 6;
            maskedPasswordTextBox.MaskInputRejected += maskedPasswordTextBox_MaskInputRejected;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(maskedPasswordTextBox);
            Controls.Add(loginButton);
            Controls.Add(passwordLabel);
            Controls.Add(userNameLabel);
            Controls.Add(loginLabel);
            Controls.Add(textboxUserName);
            Margin = new Padding(3, 2, 3, 2);
            Name = "LoginForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textboxUserName;
        private Label loginLabel;
        private Label userNameLabel;
        private Label passwordLabel;
        private Button loginButton;
        private MaskedTextBox maskedPasswordTextBox;
    }
}
