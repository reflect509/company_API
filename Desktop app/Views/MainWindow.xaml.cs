using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using Desktop_app.Views;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace Desktop_app.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private readonly Stack<UserControl> _history = new();
        public ContentControl ContentAreaControl => ContentArea; // Если его нет в свойствах

        private readonly IApiService apiService;
        private WorkerManagement workerManagementControl;
        private WorkersListControl workersListControl;
        public WorkersListControl CurrentWorkersListControl { get; set; }
        public WorkerCardViewModel CurrentWorkerCardViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            apiService = new ApiService(new HttpClient());

            // Инициализируем контролы
            workerManagementControl = new WorkerManagement();
            workersListControl = new WorkersListControl();

            // По умолчанию показываем WorkerManagement
            ContentArea.Content = workerManagementControl;
        }

        public void Navigate(UserControl page)
        {
            if (ContentArea.Content is UserControl current)
                _history.Push(current);

            ContentArea.Content = page;
        }

        public void GoBack()
        {
            if (_history.Count > 0)
                ContentArea.Content = _history.Pop();
        }

        private void OnNavigate(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            string tag = button?.Tag?.ToString();

            switch (tag)
            {
                case "WorkerManagement":
                    ContentArea.Content = workerManagementControl;
                    break;
                case "Workers":
                    ContentArea.Content = workersListControl;
                    workersListControl.RefreshWorkers();
                    break;
            }
        }

        public void NavigateBack()
        {
            ContentArea.Content = workerManagementControl;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}