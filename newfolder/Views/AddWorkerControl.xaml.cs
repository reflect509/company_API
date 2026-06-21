using Desktop_app.Models;
using Desktop_app.Services;
using System.Windows;
using System.Net.Http;
using System.Windows.Controls;

namespace Desktop_app.Views
{
    public partial class AddWorkerControl : UserControl
    {
        private IApiService apiService;
        private List<Node> subdepartments;
        private WorkersListControl previousControl; // Сохраняем WorkersList


        public AddWorkerControl()
        {
            InitializeComponent();
            apiService = new ApiService(new HttpClient());
            LoadSubdepartments();
        }

        private async void LoadSubdepartments()
        {
            try
            {
                var depts = await apiService.GetSubdepartmentsAsync();
                subdepartments = depts.ToList();
                SubdepartmentCombo.ItemsSource = subdepartments;
                SubdepartmentCombo.DisplayMemberPath = "SubdepartmentName";
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Ошибка загрузки подразделений: {ex.Message}";
            }
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameBox.Text) ||
        string.IsNullOrWhiteSpace(EmailBox.Text) ||
        string.IsNullOrWhiteSpace(JobPositionBox.Text) ||
        string.IsNullOrWhiteSpace(OfficeBox.Text) ||
        SubdepartmentCombo.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                var selectedDept = SubdepartmentCombo.SelectedItem as Node;

                var newWorker = new Worker
                {
                    FullName = FullNameBox.Text,
                    JobPosition = JobPositionBox.Text,
                    Email = EmailBox.Text,
                    WorkPhone = WorkPhoneBox.Text,
                    Phone = PhoneBox.Text,
                    Office = OfficeBox.Text,
                    SubdepartmentId = selectedDept.SubdepartmentId
                };

                apiService.CreateWorkerAsync(newWorker); // или CreateWorker если будет метод

                MessageBox.Show("Сотрудник добавлен", "Успех"); 
                if (previousControl != null)
                {
                    previousControl.RefreshWorkers();
                    //MainWindow.Instance.ContentArea.Content = previousControl;
                    MainWindow.Instance.GoBack();
                }
                else
                {
                    //var workersListControl = new WorkersListControl();
                    //workersListControl.RefreshWorkers();
                    //MainWindow.Instance.ContentArea.Content = workersListControl;
                    MainWindow.Instance.GoBack();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = $"Ошибка: {ex.Message}";
            }
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            // Возврат к списку сотрудников
            //var workersListControl = new WorkersListControl();
            //MainWindow.Instance.ContentArea.Content = workersListControl;
            MainWindow.Instance.GoBack();
        }
    }
}