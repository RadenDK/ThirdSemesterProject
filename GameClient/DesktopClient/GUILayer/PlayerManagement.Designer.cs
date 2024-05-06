namespace DesktopClient.GUILayer
{
    partial class PlayerManagement
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
            components = new System.ComponentModel.Container();
            PlayerManagementLabel = new Label();
            playerDataGridView = new DataGridView();
            PlayerId = new DataGridViewTextBoxColumn();
            usernameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            inGameNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            eloDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            emailDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            bannedDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            currencyAmountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            isOwnerDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            gameLobbyIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            onlineStatusDataGridViewCheckBoxColumn = new DataGridViewCheckBoxColumn();
            playerModelBindingSource = new BindingSource(components);
            backButton = new Button();
            selectButton = new Button();
            ((System.ComponentModel.ISupportInitialize)playerDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)playerModelBindingSource).BeginInit();
            SuspendLayout();
            // 
            // PlayerManagementLabel
            // 
            PlayerManagementLabel.AutoSize = true;
            PlayerManagementLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            PlayerManagementLabel.Location = new Point(12, 25);
            PlayerManagementLabel.Name = "PlayerManagementLabel";
            PlayerManagementLabel.Size = new Size(243, 32);
            PlayerManagementLabel.TabIndex = 1;
            PlayerManagementLabel.Text = "Player Management";
            // 
            // PlayerDataGridView
            // 
            playerDataGridView.AutoGenerateColumns = false;
            playerDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            playerDataGridView.Columns.AddRange(new DataGridViewColumn[] { PlayerId, usernameDataGridViewTextBoxColumn, inGameNameDataGridViewTextBoxColumn, eloDataGridViewTextBoxColumn, emailDataGridViewTextBoxColumn, bannedDataGridViewCheckBoxColumn, currencyAmountDataGridViewTextBoxColumn, isOwnerDataGridViewCheckBoxColumn, gameLobbyIdDataGridViewTextBoxColumn, onlineStatusDataGridViewCheckBoxColumn });
            playerDataGridView.DataSource = playerModelBindingSource;
            playerDataGridView.Location = new Point(12, 80);
            playerDataGridView.Name = "PlayerDataGridView";
            playerDataGridView.RowHeadersWidth = 62;
            playerDataGridView.Size = new Size(992, 408);
            playerDataGridView.TabIndex = 3;
            // 
            // PlayerId
            // 
            PlayerId.DataPropertyName = "PlayerId";
            PlayerId.HeaderText = "PlayerId";
            PlayerId.MinimumWidth = 8;
            PlayerId.Name = "PlayerId";
            PlayerId.ReadOnly = true;
            PlayerId.Visible = false;
            PlayerId.Width = 150;
            // 
            // usernameDataGridViewTextBoxColumn
            // 
            usernameDataGridViewTextBoxColumn.DataPropertyName = "Username";
            usernameDataGridViewTextBoxColumn.HeaderText = "Username";
            usernameDataGridViewTextBoxColumn.MinimumWidth = 8;
            usernameDataGridViewTextBoxColumn.Name = "usernameDataGridViewTextBoxColumn";
            usernameDataGridViewTextBoxColumn.ReadOnly = true;
            usernameDataGridViewTextBoxColumn.Width = 150;
            // 
            // inGameNameDataGridViewTextBoxColumn
            // 
            inGameNameDataGridViewTextBoxColumn.DataPropertyName = "InGameName";
            inGameNameDataGridViewTextBoxColumn.HeaderText = "InGameName";
            inGameNameDataGridViewTextBoxColumn.MinimumWidth = 8;
            inGameNameDataGridViewTextBoxColumn.Name = "inGameNameDataGridViewTextBoxColumn";
            inGameNameDataGridViewTextBoxColumn.ReadOnly = true;
            inGameNameDataGridViewTextBoxColumn.Width = 150;
            // 
            // eloDataGridViewTextBoxColumn
            // 
            eloDataGridViewTextBoxColumn.DataPropertyName = "Elo";
            eloDataGridViewTextBoxColumn.HeaderText = "Elo";
            eloDataGridViewTextBoxColumn.MinimumWidth = 8;
            eloDataGridViewTextBoxColumn.Name = "eloDataGridViewTextBoxColumn";
            eloDataGridViewTextBoxColumn.ReadOnly = true;
            eloDataGridViewTextBoxColumn.Width = 150;
            // 
            // emailDataGridViewTextBoxColumn
            // 
            emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            emailDataGridViewTextBoxColumn.HeaderText = "Email";
            emailDataGridViewTextBoxColumn.MinimumWidth = 8;
            emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            emailDataGridViewTextBoxColumn.ReadOnly = true;
            emailDataGridViewTextBoxColumn.Width = 150;
            // 
            // bannedDataGridViewCheckBoxColumn
            // 
            bannedDataGridViewCheckBoxColumn.DataPropertyName = "Banned";
            bannedDataGridViewCheckBoxColumn.HeaderText = "Banned";
            bannedDataGridViewCheckBoxColumn.MinimumWidth = 8;
            bannedDataGridViewCheckBoxColumn.Name = "bannedDataGridViewCheckBoxColumn";
            bannedDataGridViewCheckBoxColumn.ReadOnly = true;
            bannedDataGridViewCheckBoxColumn.Width = 150;
            // 
            // currencyAmountDataGridViewTextBoxColumn
            // 
            currencyAmountDataGridViewTextBoxColumn.DataPropertyName = "CurrencyAmount";
            currencyAmountDataGridViewTextBoxColumn.HeaderText = "CurrencyAmount";
            currencyAmountDataGridViewTextBoxColumn.MinimumWidth = 8;
            currencyAmountDataGridViewTextBoxColumn.Name = "currencyAmountDataGridViewTextBoxColumn";
            currencyAmountDataGridViewTextBoxColumn.ReadOnly = true;
            currencyAmountDataGridViewTextBoxColumn.Width = 150;
            // 
            // isOwnerDataGridViewCheckBoxColumn
            // 
            isOwnerDataGridViewCheckBoxColumn.DataPropertyName = "IsOwner";
            isOwnerDataGridViewCheckBoxColumn.HeaderText = "IsOwner";
            isOwnerDataGridViewCheckBoxColumn.MinimumWidth = 8;
            isOwnerDataGridViewCheckBoxColumn.Name = "isOwnerDataGridViewCheckBoxColumn";
            isOwnerDataGridViewCheckBoxColumn.ReadOnly = true;
            isOwnerDataGridViewCheckBoxColumn.Visible = false;
            isOwnerDataGridViewCheckBoxColumn.Width = 150;
            // 
            // gameLobbyIdDataGridViewTextBoxColumn
            // 
            gameLobbyIdDataGridViewTextBoxColumn.DataPropertyName = "GameLobbyId";
            gameLobbyIdDataGridViewTextBoxColumn.HeaderText = "GameLobbyId";
            gameLobbyIdDataGridViewTextBoxColumn.MinimumWidth = 8;
            gameLobbyIdDataGridViewTextBoxColumn.Name = "gameLobbyIdDataGridViewTextBoxColumn";
            gameLobbyIdDataGridViewTextBoxColumn.ReadOnly = true;
            gameLobbyIdDataGridViewTextBoxColumn.Visible = false;
            gameLobbyIdDataGridViewTextBoxColumn.Width = 150;
            // 
            // onlineStatusDataGridViewCheckBoxColumn
            // 
            onlineStatusDataGridViewCheckBoxColumn.DataPropertyName = "OnlineStatus";
            onlineStatusDataGridViewCheckBoxColumn.HeaderText = "OnlineStatus";
            onlineStatusDataGridViewCheckBoxColumn.MinimumWidth = 8;
            onlineStatusDataGridViewCheckBoxColumn.Name = "onlineStatusDataGridViewCheckBoxColumn";
            onlineStatusDataGridViewCheckBoxColumn.ReadOnly = true;
            onlineStatusDataGridViewCheckBoxColumn.Visible = false;
            onlineStatusDataGridViewCheckBoxColumn.Width = 150;
            // 
            // playerModelBindingSource
            // 
            playerModelBindingSource.DataSource = typeof(ModelLayer.PlayerModel);
            // 
            // backButton
            // 
            backButton.Location = new Point(774, 494);
            backButton.Name = "backButton";
            backButton.Size = new Size(112, 34);
            backButton.TabIndex = 5;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = true;
            // 
            // selectButton
            // 
            selectButton.Location = new Point(892, 494);
            selectButton.Name = "selectButton";
            selectButton.Size = new Size(112, 34);
            selectButton.TabIndex = 6;
            selectButton.Text = "Select";
            selectButton.UseVisualStyleBackColor = true;
            // 
            // PlayerManagement
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1016, 540);
            Controls.Add(selectButton);
            Controls.Add(backButton);
            Controls.Add(playerDataGridView);
            Controls.Add(PlayerManagementLabel);
            Name = "PlayerManagement";
            Text = "PlayerManagement";
            ((System.ComponentModel.ISupportInitialize)playerDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)playerModelBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label PlayerManagementLabel;
        private DataGridView playerDataGridView;
        private BindingSource playerModelBindingSource;
        private Button backButton;
        private DataGridViewTextBoxColumn PlayerId;
        private DataGridViewTextBoxColumn usernameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn inGameNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn eloDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn bannedDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn currencyAmountDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn isOwnerDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn gameLobbyIdDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn onlineStatusDataGridViewCheckBoxColumn;
        private Button selectButton;
    }
}