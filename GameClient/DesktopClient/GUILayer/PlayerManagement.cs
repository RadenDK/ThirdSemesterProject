using DesktopClient.ControllerLayer;
using DesktopClient.ModelLayer;

using System.Data;


namespace DesktopClient.GUILayer
{
	public partial class PlayerManagement : Form
	{
		private readonly PlayerController _playerController;
		private List<PlayerModel> _players;
		private ApplicationContextManager _applicationContextManager;

		public PlayerManagement(ApplicationContextManager applicationContextManager, PlayerController playerController)
		{
			InitializeComponent();
			_playerController = playerController;
			_players = new List<PlayerModel>();
			this.Load += PopulatePlayerList;
			playerDataGridView.CellDoubleClick += PlayerDataGridView_CellDoubleClick;
			selectButton.Click += SelectButton_Click;
			searchTextBox.TextChanged += searchTextBox_TextChanged;
			reloadPictureBox.Click += ReloadPictureBox_Click;

			_applicationContextManager = applicationContextManager;
		}

		private async void PopulatePlayerList(object sender, EventArgs e)
		{
			_players = await _playerController.GetAllPlayers();
			playerDataGridView.DataSource = _players;
		}

		private void SelectButton_Click(object sender, EventArgs e)
		{
			PlayerModel selectedPlayer = playerDataGridView.CurrentRow.DataBoundItem as PlayerModel;
			if (selectedPlayer != null)
			{
				EditPlayerForm editPlayerForm = new EditPlayerForm(_playerController, selectedPlayer);
				editPlayerForm.PlayerEdited += UpdatePlayerList;
				editPlayerForm.ShowDialog();
			}
		}

        private void PlayerDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			PlayerModel selectedPlayer = playerDataGridView.CurrentRow.DataBoundItem as PlayerModel;
			if (selectedPlayer != null)
			{
				EditPlayerForm editPlayerForm = new EditPlayerForm(_playerController, selectedPlayer);
                editPlayerForm.PlayerEdited += UpdatePlayerList;
                editPlayerForm.ShowDialog();
			}
		}

        private void UpdatePlayerList()
        {
            PopulatePlayerList(this, EventArgs.Empty);
        }

        private async void searchTextBox_TextChanged(object sender, EventArgs e)
		{
			FilterPlayer();
		}

		private void FilterPlayer()
		{
			string searchInput = searchTextBox.Text.ToLower();
			List<PlayerModel> filteredPlayers;

			if (string.IsNullOrEmpty(searchInput))
			{
				filteredPlayers = _players;
			}
			else
			{
				filteredPlayers = _players.Where(p => p.Username.ToLower().Contains(searchInput) || p.InGameName.ToLower().Contains(searchInput) || p.Email.ToLower().Contains(searchInput)).ToList();
			}
			playerDataGridView.DataSource = filteredPlayers;
		}

		private async void ReloadPictureBox_Click(object sender, EventArgs e)
		{
			PopulatePlayerList(sender, e);
		}

		private void backButton_Click(object sender, EventArgs e)
		{
			_applicationContextManager.ShowAdminDashboardForm();
		}
	}
}
