using Desktop_app.Services;
using System.Windows;
using System.Net.Http;

namespace Desktop_app.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage.Text = "Введите логин и пароль";
                return;
            }

            ErrorMessage.Text = "";

            try
            {
                var apiService = new ApiService(new HttpClient());
                bool isAuthenticated = await apiService.LoginAsync(username, password);

                if (isAuthenticated)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    ErrorMessage.Text = "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Ошибка: {ex.Message}";
            }
        }
    }
}