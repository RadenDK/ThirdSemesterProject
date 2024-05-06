using DesktopClient.ControllerLayer;

namespace DesktopClient
{
    public partial class LoginForm : Form
    {

        private AdminController _adminController;
        public LoginForm(AdminController adminController)
        {
            InitializeComponent();
            _adminController = adminController;
        }


        private async void loginButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textboxAdminId.Text, out int adminId))
            {
                MessageBox.Show("Enter valid admin id");
                return;
            }

            string password = maskedPasswordTextBox.Text;

            try
            {
                bool loginSuccessful = await _adminController.VerifyLogin(adminId, password);

                if (loginSuccessful)
                {
                    MessageBox.Show("Login Successful");
                    // Proceed to the next part of your application
                }
                else
                {
                    MessageBox.Show("Login Failed. Please check your credentials.");
                }
            } // Handles Exceptions from the VerifyLogin method
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }






        private void passwordLabel_Click(object sender, EventArgs e)
        {

        }

        private void userNameLabel_Click(object sender, EventArgs e)
        {

        }

        private void maskedPasswordTextBox_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
} 

