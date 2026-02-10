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
        private bool _isFormatting;
        public WorkerCard(IApiService apiService, Worker selectedWorker, ObservableCollection<Worker> workers)
        {
            InitializeComponent();

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
    }
}
