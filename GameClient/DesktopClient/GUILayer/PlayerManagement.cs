using DesktopClient.ControllerLayer;
using DesktopClient.ModelLayer;
using DesktopClient.ServiceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient.GUILayer
{
    public partial class PlayerManagement : Form
    {
        private readonly PlayerController _playerController;

        public PlayerManagement(IPlayerService playerService)
        {
            InitializeComponent();
            _playerController = new PlayerController(playerService);
            this.Load += PopulatePlayerList;
            ConfirmButton.Click += ConfirmButton_Click;
        }

        private async void PopulatePlayerList(object sender, EventArgs e)
        {
            List<PlayerModel> players = await _playerController.GetAllPlayers();
            PlayerDataGridView.DataSource = players;
        }

        private async void ConfirmButton_Click(object sender, EventArgs e)
        {
            var selectedPlayer = PlayerDataGridView.CurrentRow.DataBoundItem as PlayerModel;

            if (selectedPlayer != null)
            {
                selectedPlayer.Banned = true;

                var result = await _playerController.BanPlayer(selectedPlayer);

                if (result)
                {
                    MessageBox.Show("Player: " + selectedPlayer.Username + "banned successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to ban player.");
                }
            }
        }
    }
}
