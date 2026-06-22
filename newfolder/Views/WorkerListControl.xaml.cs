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


        public WorkersListControl()
        {
            InitializeComponent();
            apiService = new ApiService(new HttpClient());
            MainWindow.Instance.CurrentWorkersListControl = this; // Ссылка на текущий контрол для обновления списка
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

            RefreshWorkers();
        }

        private void LoadWorkers()
        {
            vm.LoadAllWorkers();
        }

        private void OnAddWorkerClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.Navigate(new AddWorkerControl());
        }

        private void OpenWorkerCard(Worker worker)
        {
            MainWindow.Instance.Navigate(new WorkerCard(apiService, worker, vm.Workers));
        }

        public void RefreshWorkers()
        {
            LoadWorkers();
        }
    }
}