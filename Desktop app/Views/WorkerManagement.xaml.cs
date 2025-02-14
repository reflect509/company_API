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
            DataContext = new WorkerManagementViewModel(new ApiService(new HttpClient()));

            AddElementsToCanvas();
        }

        private void AddElementsToCanvas()
        {
            var viewmodel = DataContext as WorkerManagementViewModel;

            foreach (var node in viewmodel.Nodes)
            {
                Button nodeButton = new Button
                {
                    Content = node.Name,
                    Width = 100,
                    Height = 50
                };

                Canvas.SetLeft(nodeButton, node.X);
                Canvas.SetTop(nodeButton, node.Y);
                SubdepartmentCanvas.Children.Add(nodeButton);
            }

            foreach (var line in viewmodel.Lines)
            {
                SubdepartmentCanvas.Children.Add(line);
            }
        }
    }
}