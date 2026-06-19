using Desktop_app.Models;
using Desktop_app.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using Desktop_app.ViewModels;

namespace Desktop_app.Views
{
    public partial class WorkersListControl : UserControl
    {
        private WorkerManagementViewModel vm;
        private IApiService apiService;
        private WorkersListControl previousControl; // Сохраняем WorkersList


        public WorkersListControl()
        {
            InitializeComponent();
            apiService = new ApiService(new HttpClient());
            vm = new WorkerManagementViewModel(apiService);
            this.DataContext = vm;

            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(vm.SelectedWorker) && vm.SelectedWorker != null)
                {
                    OpenWorkerCard(vm.SelectedWorker);
                    vm.SelectedWorker = null; // Сбрасываем выделение
                }
            };

            LoadWorkers();
        }

        private void LoadWorkers()
        {
            vm.LoadAllWorkers();
        }

        private void OnAddWorkerClicked(object sender, RoutedEventArgs e)
        {
            var addWorkerControl = new AddWorkerControl(); // Передаём this
            MainWindow.Instance.ContentArea.Content = addWorkerControl;
        }

        private void OpenWorkerCard(Worker worker)
        {
            var workerCard = new WorkerCard(apiService, worker, vm.Workers, "WorkersList");
            MainWindow.Instance.ContentArea.Content = workerCard;
        }

        public void RefreshWorkers()
        {
            LoadWorkers();
        }
    }
}