using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using Desktop_app.Views;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace Desktop_app.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public ContentControl ContentAreaControl => ContentArea; // Если его нет в свойствах

        private readonly IApiService apiService;
        private WorkerManagement workerManagementControl;
        private WorkerCard workerCardControl;
        private CreateEvent eventCardControl;
        private WorkersListControl workersListControl;
        private AddWorkerControl addWorkerControl;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;

            apiService = new ApiService(new HttpClient());

            // Инициализируем контролы
            workerManagementControl = new WorkerManagement();
            workersListControl = new WorkersListControl();
            addWorkerControl = new AddWorkerControl();

            // По умолчанию показываем WorkerManagement
            ContentArea.Content = workerManagementControl;
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

        public void NavigateToWorkerCard(WorkerCard card)
{
    workerCardControl = card;
    ContentArea.Content = workerCardControl;
}

        public void NavigateToAddWorker()
        {
            ContentArea.Content = addWorkerControl;
        }

        public void NavigateToWorkersList()
        {
            ContentArea.Content = workersListControl;
            workersListControl.RefreshWorkers();
        }

        public void NavigateToCreateEvent(Worker worker)
        {
            eventCardControl = new CreateEvent(apiService, worker);
            ContentArea.Content = eventCardControl;
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