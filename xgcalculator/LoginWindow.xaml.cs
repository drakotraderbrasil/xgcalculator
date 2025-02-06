using System;
using System.Threading.Tasks;
using System.Windows;

namespace XGCalculator
{
    public partial class LoginWindow : Window
    {
        private readonly APIClient _apiClient;

        public LoginWindow()
        {
            InitializeComponent();
            _apiClient = new APIClient();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, preencha usuário e senha.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool loginSuccess = await _apiClient.Login(username, password);
            if (loginSuccess)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Falha no login. Verifique suas credenciais.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
