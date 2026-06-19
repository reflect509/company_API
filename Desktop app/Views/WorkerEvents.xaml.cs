using Desktop_app.Models;
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
        private UserControl previousControl;
        public WorkerEvents()
        {
            InitializeComponent();
        }
        public void SetWorker(Worker worker,UserControl previousControl = null)
        {
            selectedWorker = worker;
            this.previousControl = previousControl;
            TitleBlock.Text = $"События сотрудника: {worker.FullName}";

            // Просто устанавливаем ItemsSource напрямую
            if (worker.Events != null && worker.Events.Count > 0)
            {
                // Если есть события — показываем их
                EventsDataGrid.ItemsSource = worker.Events;
                EmptyMessage.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Если нет событий — показываем сообщение
                EmptyMessage.Visibility = Visibility.Visible;
            }
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            if (previousControl != null)
            {
                MainWindow.Instance.ContentArea.Content = previousControl;
            }
            else
            {
                MainWindow.Instance.NavigateBack();
            }
        }
    }
}
