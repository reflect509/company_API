using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_app
{
    /// <summary>
    /// Interaction logic for WorkerManagement.xaml
    /// </summary>
    public partial class WorkerManagement : Window
    {
        private double _scaleValue = 1.0;

        private bool _subdepartmentGridIsFullscreen;
        private int _subdepartmentGridOriginalRow;
        private int _subdepartmentGridOriginalColumn;
        private int _subdepartmentGridOriginalRowSpan;
        private int _subdepartmentGridOriginalColumnSpan;
        public WorkerManagement()
        {
            InitializeComponent();
            var viewModel = new WorkerManagementViewModel(new ApiService(new HttpClient()));
            this.DataContext = viewModel;

            
            viewModel.DataLoaded += ViewModel_DataLoaded;
            viewModel.ExitFullscreenRequested += OnExitFullscreenRequested;
        }
                
        private void OnExitFullscreenRequested()
        {
            if (_subdepartmentGridIsFullscreen)
            {
                ExitFullscreen();
                _subdepartmentGridIsFullscreen = false;
            }
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == 0)
                return;

            e.Handled = true;

            var scrollViewer = (ScrollViewer)sender;

            double oldScale = _scaleValue;
            double zoomDelta = e.Delta > 0 ? 0.1 : -0.1;
            _scaleValue = Math.Clamp(_scaleValue + zoomDelta, 0.1, 10.0);

            if (Math.Abs(oldScale - _scaleValue) < 0.00001)
                return;

            // позиция мыши относительно ScrollViewer (ВАЖНО)
            Point mousePos = e.GetPosition(scrollViewer);

            double offsetX = scrollViewer.HorizontalOffset;
            double offsetY = scrollViewer.VerticalOffset;

            // применяем масштаб
            SubdepartmentCanvas.LayoutTransform =
                new ScaleTransform(_scaleValue, _scaleValue);

            double scaleRatio = _scaleValue / oldScale;

            // корректные offsets
            scrollViewer.ScrollToHorizontalOffset(
                (offsetX + mousePos.X) * scaleRatio - mousePos.X);

            scrollViewer.ScrollToVerticalOffset(
                (offsetY + mousePos.Y) * scaleRatio - mousePos.Y);
        }

        private void ToggleFullscreen()
        {
            if (!_subdepartmentGridIsFullscreen)
            {
                EnterFullscreen();
            }
            else
            {
                ExitFullscreen();
            }

            _subdepartmentGridIsFullscreen = !_subdepartmentGridIsFullscreen;
        }

        private void EnterFullscreen()
        {
            // сохраняем состояние
            _subdepartmentGridOriginalRow = Grid.GetRow(SubdepartmentGrid);
            _subdepartmentGridOriginalColumn = Grid.GetColumn(SubdepartmentGrid);
            _subdepartmentGridOriginalRowSpan = Grid.GetRowSpan(SubdepartmentGrid);
            _subdepartmentGridOriginalColumnSpan = Grid.GetColumnSpan(SubdepartmentGrid);

            // растягиваем на весь Grid
            Grid.SetRow(SubdepartmentGrid, 0);
            Grid.SetColumn(SubdepartmentGrid, 0);
            Grid.SetRowSpan(SubdepartmentGrid, MainGrid.RowDefinitions.Count);
            Grid.SetColumnSpan(SubdepartmentGrid, MainGrid.ColumnDefinitions.Count);

            // скрываем остальные элементы
            WorkerListBox.Visibility = Visibility.Hidden;
            TopPanel.Visibility = Visibility.Hidden;
        }

        private void ExitFullscreen()
        {
            Grid.SetRow(SubdepartmentGrid, _subdepartmentGridOriginalRow);
            Grid.SetColumn(SubdepartmentGrid, _subdepartmentGridOriginalColumn);
            Grid.SetRowSpan(SubdepartmentGrid, _subdepartmentGridOriginalRowSpan);
            Grid.SetColumnSpan(SubdepartmentGrid, _subdepartmentGridOriginalColumnSpan);

            WorkerListBox.Visibility = Visibility.Visible;
            TopPanel.Visibility = Visibility.Visible;       
        }
        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleFullscreen();
        }   
    }
}