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
    public partial class WorkerCard : UserControl
    {
        private bool _isFormatting;

        private IApiService apiService;
        private Worker selectedWorker;
        private string previousScreen;
        private UserControl previousControl;
        public WorkerCard(IApiService apiService, Worker selectedWorker, 
            ObservableCollection<Worker> workers, string previousScreen = "WorkerManagement",
            UserControl previousControl = null)
        {
            InitializeComponent();
            this.apiService = apiService;
            this.selectedWorker = selectedWorker;
            this.previousScreen = previousScreen;
            this.previousControl = previousControl;

            this.DataContext = new WorkerCardViewModel(apiService, selectedWorker, workers);
        }

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void Phone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFormatting)
                return;

            var tb = (TextBox)sender;

            // Если пользователь удаляет — НЕ форматируем
            if (e.Changes.Any(c => c.RemovedLength > 0))
                return;

            var digits = new string(tb.Text.Where(char.IsDigit).ToArray());

            if (digits.StartsWith("7"))
                digits = digits[1..];

            _isFormatting = true;
            tb.Text = FormatPhone(digits);
            tb.CaretIndex = tb.Text.Length;
            _isFormatting = false;
        }

        private string FormatPhone(string digits)
        {
            if (digits.Length > 10)
                digits = digits[..10];

            if (digits.Length < 3)
                return "+7 (" + digits;

            if (digits.Length < 6)
                return $"+7 ({digits[..3]}) {digits[3..]}";

            if (digits.Length < 8)
                return $"+7 ({digits[..3]}) {digits[3..6]}-{digits[6..]}";

            return $"+7 ({digits[..3]}) {digits[3..6]}-{digits[6..8]}-{digits[8..]}";
        }

        private void OnBackClicked(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.GoBack();
        }

        private void OnEventsClicked(object sender, RoutedEventArgs e)
        {
            var vm = (WorkerCardViewModel)this.DataContext;

            if (vm.SelectedWorker == null)
            {
                MessageBox.Show("Сотрудник не выбран", "Ошибка");
                return;
            }

            var eventsControl = new WorkerEvents();
            eventsControl.SetWorker(vm.Workers[0], this);
            MainWindow.Instance.Navigate(eventsControl);
        }

        private async void OnDeleteWorkerClicked(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                $"Вы уверены, что хотите удалить сотрудника {selectedWorker.FullName}?",
                "Подтверждение удаления",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                bool success = await apiService.DeleteWorkerAsync(selectedWorker.WorkerId);

                if (success)
                {
                    MessageBox.Show("Сотрудник успешно удалён", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (previousScreen == "WorkersList")
                    {
                        //var workersListControl = new WorkersListControl();
                        //workersListControl.RefreshWorkers();
                        //MainWindow.Instance.ContentArea.Content = workersListControl;
                        MainWindow.Instance.GoBack();
                    }
                    else
                    {
                        MainWindow.Instance.GoBack();
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
