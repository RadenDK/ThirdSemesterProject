using DesktopClient.ControllerLayer;
using DesktopClient.ModelLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient.GUILayer
{
    public partial class EditPlayerForm : Form
    {
        private PlayerController _playerController;
        private PlayerModel _player;
        public event Action PlayerEdited;
        public EditPlayerForm(PlayerController playerController, PlayerModel player)
        {
            InitializeComponent();
            _playerController = playerController;
            _player = player;

            usernameTextBox.Text = _player.Username;
            inGameNameTextBox.Text = _player.InGameName;
            eloTextBox.Text = _player.Elo.ToString();
            emailTextBox.Text = _player.Email;
            currencyTextBox.Text = _player.CurrencyAmount.ToString();
            bannedCheckBox.Checked = _player.Banned;
        }



        private void backButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		private async void confirmButton_Click(object sender, EventArgs e)
		{
			if (!ChangesCheck())
			{
				MessageBox.Show("No changes was made", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			if (int.Parse(currencyTextBox.Text) < 0)
			{
				MessageBox.Show("Invalid amount for currency", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				currencyTextBox.Text = _player.CurrencyAmount.ToString();
				return;
			}
			if (int.Parse(eloTextBox.Text) < 0)
			{
				MessageBox.Show("Invalid amount for elo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				eloTextBox.Text = _player.Elo.ToString();
				return;
			}

			if (bannedCheckBox.Checked)
			{
				_player.Banned = true;
			}
			if (!bannedCheckBox.Checked)
			{
				_player.Banned = false;
			}

			_player.Username = usernameTextBox.Text;
			_player.InGameName = inGameNameTextBox.Text;
			_player.Email = emailTextBox.Text;
			_player.Elo = int.Parse(eloTextBox.Text);
			_player.CurrencyAmount = int.Parse(currencyTextBox.Text);

			bool result = await _playerController.UpdatePlayer(_player);

			if (result)
			{
				MessageBox.Show("Updated player successfully", "Update success", MessageBoxButtons.OK, MessageBoxIcon.Information);
				PlayerEdited?.Invoke();
				this.Close();
			}
			else
			{
				MessageBox.Show("Failed to update player", "Update error", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
		}

        private bool ChangesCheck()
        {
            bool isUsernameChanged = !usernameTextBox.Text.Equals(_player.Username);
            bool isInGameNameChanged = !inGameNameTextBox.Text.Equals(_player.InGameName);
            bool isEloChanged = !eloTextBox.Text.Equals(_player.Elo.ToString());
            bool isEmailChanged = !emailTextBox.Text.Equals(_player.Email);
            bool isCurrencyChanged = !currencyTextBox.Text.Equals(_player.CurrencyAmount.ToString());
            bool isBannedChanged = bannedCheckBox.Checked != _player.Banned;

            return isUsernameChanged || isInGameNameChanged || isEloChanged || isEmailChanged || isCurrencyChanged || isBannedChanged;
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this player?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                bool result = await _playerController.DeletePlayer(_player);
                if (result)
                {
                    PlayerEdited?.Invoke();
                    MessageBox.Show(_player.Username + " was successfully deleted.", "Delete Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show(_player.Username + " was not deleted.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
