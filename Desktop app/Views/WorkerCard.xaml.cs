using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Desktop_app.Views
{
    /// <summary>
    /// Interaction logic for WorkerCard.xaml
    /// </summary>
    public partial class WorkerCard : Window
    {
        public WorkerCard(IApiService apiService, Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            InitializeComponent();
            DataContext = new WorkerCardViewModel(apiService, selectedWorker, workers);
            if (SupervisorComboBox.SelectedItem == null)
            {
                SupervisorComboBox.SelectedItem = "Выберите руководителя работника";
            }

            if (SupervisorSupportComboBox.SelectedItem == null)
            {
                SupervisorSupportComboBox.SelectedItem = "Выберите помощника работника";
            }
        }

        private void ApplyChanges(object sender, RoutedEventArgs e)
        {

        }
    }
}
