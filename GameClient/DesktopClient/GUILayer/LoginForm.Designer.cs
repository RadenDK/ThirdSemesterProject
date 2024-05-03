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
            textBoxPassword = new TextBox();
            textboxUserName = new TextBox();
            loginLabel = new Label();
            userNameLabel = new Label();
            passwordLabel = new Label();
            SuspendLayout();
            // 
            // textBoxPassword
            // 
            textBoxPassword.Location = new Point(293, 236);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(274, 27);
            textBoxPassword.TabIndex = 0;
            // 
            // textboxUserName
            // 
            textboxUserName.Location = new Point(293, 147);
            textboxUserName.Name = "textboxUserName";
            textboxUserName.Size = new Size(274, 27);
            textboxUserName.TabIndex = 1;
            // 
            // loginLabel
            // 
            loginLabel.AutoSize = true;
            loginLabel.Location = new Point(293, 50);
            loginLabel.Name = "loginLabel";
            loginLabel.Size = new Size(46, 20);
            loginLabel.TabIndex = 2;
            loginLabel.Text = "Login";
            // 
            // userNameLabel
            // 
            userNameLabel.AutoSize = true;
            userNameLabel.Location = new Point(293, 124);
            userNameLabel.Name = "userNameLabel";
            userNameLabel.Size = new Size(78, 20);
            userNameLabel.TabIndex = 3;
            userNameLabel.Text = "Username:";
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Location = new Point(293, 213);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Size = new Size(73, 20);
            passwordLabel.TabIndex = 4;
            passwordLabel.Text = "Password:";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(passwordLabel);
            Controls.Add(userNameLabel);
            Controls.Add(loginLabel);
            Controls.Add(textboxUserName);
            Controls.Add(textBoxPassword);
            Name = "LoginForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxPassword;
        private TextBox textboxUserName;
        private Label loginLabel;
        private Label userNameLabel;
        private Label passwordLabel;
    }
}
