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
			if (bannedCheckBox.Checked)
			{
				bool result = await _playerController.BanPlayer(_player);
				if (result)
				{
					MessageBox.Show(_player.Username + " was successfully banned.", "Ban Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
					this.Close();
				}
                else
                {
					MessageBox.Show(_player.Username + " was not banned.", "Ban Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
		}
	}
}
