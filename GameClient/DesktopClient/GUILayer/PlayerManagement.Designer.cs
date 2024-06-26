﻿namespace DesktopClient.GUILayer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerManagement));
			PlayerManagementLabel = new Label();
			playerDataGridView = new DataGridView();
			playerModelBindingSource = new BindingSource(components);
			backButton = new Button();
			selectButton = new Button();
			searchTextBox = new TextBox();
			reloadPictureBox = new PictureBox();
			managementPanel = new Panel();
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
			((System.ComponentModel.ISupportInitialize)playerDataGridView).BeginInit();
			((System.ComponentModel.ISupportInitialize)playerModelBindingSource).BeginInit();
			((System.ComponentModel.ISupportInitialize)reloadPictureBox).BeginInit();
			managementPanel.SuspendLayout();
			SuspendLayout();
			// 
			// PlayerManagementLabel
			// 
			PlayerManagementLabel.AutoSize = true;
			PlayerManagementLabel.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
			PlayerManagementLabel.Location = new Point(3, 15);
			PlayerManagementLabel.Margin = new Padding(2, 0, 2, 0);
			PlayerManagementLabel.Name = "PlayerManagementLabel";
			PlayerManagementLabel.Size = new Size(202, 28);
			PlayerManagementLabel.TabIndex = 1;
			PlayerManagementLabel.Text = "Player Management";
			// 
			// playerDataGridView
			// 
			playerDataGridView.AutoGenerateColumns = false;
			playerDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			playerDataGridView.Columns.AddRange(new DataGridViewColumn[] { PlayerId, usernameDataGridViewTextBoxColumn, inGameNameDataGridViewTextBoxColumn, eloDataGridViewTextBoxColumn, emailDataGridViewTextBoxColumn, bannedDataGridViewCheckBoxColumn, currencyAmountDataGridViewTextBoxColumn, isOwnerDataGridViewCheckBoxColumn, gameLobbyIdDataGridViewTextBoxColumn, onlineStatusDataGridViewCheckBoxColumn });
			playerDataGridView.DataSource = playerModelBindingSource;
			playerDataGridView.Location = new Point(2, 101);
			playerDataGridView.Margin = new Padding(2, 3, 2, 3);
			playerDataGridView.Name = "playerDataGridView";
			playerDataGridView.RowHeadersWidth = 62;
			playerDataGridView.Size = new Size(1035, 535);
			playerDataGridView.TabIndex = 3;
			// 
			// playerModelBindingSource
			// 
			playerModelBindingSource.DataSource = typeof(ModelLayer.PlayerModel);
			// 
			// backButton
			// 
			backButton.Location = new Point(854, 641);
			backButton.Margin = new Padding(2, 3, 2, 3);
			backButton.Name = "backButton";
			backButton.Size = new Size(90, 27);
			backButton.TabIndex = 5;
			backButton.Text = "Back";
			backButton.UseVisualStyleBackColor = true;
			backButton.Click += backButton_Click;
			// 
			// selectButton
			// 
			selectButton.Location = new Point(947, 641);
			selectButton.Margin = new Padding(2, 3, 2, 3);
			selectButton.Name = "selectButton";
			selectButton.Size = new Size(90, 27);
			selectButton.TabIndex = 6;
			selectButton.Text = "Select";
			selectButton.UseVisualStyleBackColor = true;
			// 
			// searchTextBox
			// 
			searchTextBox.ForeColor = Color.Gray;
			searchTextBox.Location = new Point(3, 65);
			searchTextBox.Name = "searchTextBox";
			searchTextBox.Size = new Size(660, 27);
			searchTextBox.TabIndex = 7;
			searchTextBox.Text = "Search...";
			searchTextBox.TextChanged += searchTextBox_TextChanged;
			// 
			// reloadPictureBox
			// 
			reloadPictureBox.Cursor = Cursors.Hand;
			reloadPictureBox.Image = (Image)resources.GetObject("reloadPictureBox.Image");
			reloadPictureBox.Location = new Point(1019, 77);
			reloadPictureBox.Name = "reloadPictureBox";
			reloadPictureBox.Size = new Size(18, 19);
			reloadPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			reloadPictureBox.TabIndex = 8;
			reloadPictureBox.TabStop = false;
			reloadPictureBox.Click += ReloadPictureBox_Click;
			// 
			// managementPanel
			// 
			managementPanel.Anchor = AnchorStyles.None;
			managementPanel.Controls.Add(selectButton);
			managementPanel.Controls.Add(PlayerManagementLabel);
			managementPanel.Controls.Add(searchTextBox);
			managementPanel.Controls.Add(reloadPictureBox);
			managementPanel.Controls.Add(backButton);
			managementPanel.Controls.Add(playerDataGridView);
			managementPanel.Location = new Point(14, 16);
			managementPanel.Margin = new Padding(3, 4, 3, 4);
			managementPanel.Name = "managementPanel";
			managementPanel.Size = new Size(1041, 671);
			managementPanel.TabIndex = 9;
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
			emailDataGridViewTextBoxColumn.Width = 200;
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
			// PlayerManagement
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1069, 703);
			Controls.Add(managementPanel);
			Margin = new Padding(2, 3, 2, 3);
			Name = "PlayerManagement";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Player Management";
			((System.ComponentModel.ISupportInitialize)playerDataGridView).EndInit();
			((System.ComponentModel.ISupportInitialize)playerModelBindingSource).EndInit();
			((System.ComponentModel.ISupportInitialize)reloadPictureBox).EndInit();
			managementPanel.ResumeLayout(false);
			managementPanel.PerformLayout();
			ResumeLayout(false);
		}

		#endregion
		private Label PlayerManagementLabel;
        private DataGridView playerDataGridView;
        private BindingSource playerModelBindingSource;
        private Button backButton;
        private Button selectButton;
		private TextBox searchTextBox;
		private PictureBox reloadPictureBox;
		private Panel managementPanel;
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
	}
}