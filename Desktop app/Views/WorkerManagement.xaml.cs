using Desktop_app.Services;
using Desktop_app.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Desktop_app.Models;

namespace Desktop_app
{
    /// <summary>
    /// Interaction logic for WorkerManagement.xaml
    /// </summary>
    public partial class WorkerManagement : Window
    {
        public WorkerManagement()
        {
            InitializeComponent();
            var viewModel = new WorkerManagementViewModel(new ApiService(new HttpClient()));
            this.DataContext = viewModel;

            
            viewModel.DataLoaded += ViewModel_DataLoaded;
        }

        private void ViewModel_DataLoaded(object? sender, EventArgs e)
        {
            SetCanvasSize();
        }

        private void SetCanvasSize()
        {
            var viewModel = (WorkerManagementViewModel)this.DataContext;

            double maxX = viewModel.Nodes.Any() ? viewModel.Nodes.Max(n => n.X) + 150 : 800;
            double maxY = viewModel.Nodes.Any() ? viewModel.Nodes.Max(n => n.Y) + 100 : 600;

            SubdepartmentCanvas.Width = maxX;
            SubdepartmentCanvas.Height = maxY;
        }

        private void Subdepartment_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
        }
    }
}