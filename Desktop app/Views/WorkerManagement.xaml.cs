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
        private double canvasScale = 1;
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

            double maxX = viewModel.Nodes.Any() ? viewModel.Nodes.Max(n => n.X) + 450 : 800;
            double maxY = viewModel.Nodes.Any() ? viewModel.Nodes.Max(n => n.Y) + 100 : 600;

            SubdepartmentCanvas.Width = maxX;
            SubdepartmentCanvas.Height = maxY;

        }

        private void Subdepartment_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (WorkerManagementViewModel)this.DataContext;
        }

        private void SubdepartmentCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            canvasScale += e.Delta > 0 ? 0.05 : -0.05;
            canvasScale = Math.Max(0.1, Math.Min(canvasScale, 10));

            Point mousePos = e.GetPosition(SubdepartmentCanvas);

            ScaleTransform scaleTransform = new ScaleTransform(canvasScale, canvasScale);
            SubdepartmentCanvas.RenderTransform = scaleTransform;
        }
    }
}