using Desktop_app.Models;
using Desktop_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_app.Views
{
    /// <summary>
    /// Interaction logic for WorkerEvents.xaml
    /// </summary>
    public partial class WorkerEvents : UserControl
    {
        private Worker selectedWorker;
        private IApiService apiService;
        private List<Event> events;
        public WorkerEvents(IApiService apiService, Worker worker)
        {
            InitializeComponent();
            this.apiService = apiService;
            selectedWorker = worker;
            TitleBlock.Text = $"События сотрудника: {worker.FullName}";
            SetEvents();
        }

        public async Task SetEvents()
        {
            await RefreshEvents();
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.GoBack();
        }

        public async Task RefreshEvents()
        {
            var events = await apiService.GetWorkerEventsAsync(selectedWorker.WorkerId);
            this.events = events;

            if (events != null && events.Count > 0)
            {
                // Если есть события — показываем их
                EventsDataGrid.ItemsSource = events;
                EmptyMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Если нет событий — показываем сообщение
                EmptyMessage.Visibility = Visibility.Visible;
            }
        }
    }
}
