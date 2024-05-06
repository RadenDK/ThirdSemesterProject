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

        public PlayerManagement(PlayerController playerController)
        {
            InitializeComponent();
            _playerController = playerController;
            this.Load += PopulatePlayerList;
            playerDataGridView.CellDoubleClick += PlayerDataGridView_CellDoubleClick;
            selectButton.Click += SelectButton_Click;

        }

        private async void PopulatePlayerList(object sender, EventArgs e)
        {
            List<PlayerModel> players = await _playerController.GetAllPlayers();
            playerDataGridView.DataSource = players;
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            var selectedPlayer = playerDataGridView.CurrentRow.DataBoundItem as PlayerModel;
            if (selectedPlayer != null)
            {
                EditPlayerForm editPlayerForm = new EditPlayerForm(selectedPlayer);
                editPlayerForm.Show();
            }
        }

        private void PlayerDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var selectedPlayer = playerDataGridView.CurrentRow.DataBoundItem as PlayerModel;
            if (selectedPlayer != null)
            {
                EditPlayerForm editPlayerForm = new EditPlayerForm(selectedPlayer);
                editPlayerForm.Show();
            }
        }
    }
}
