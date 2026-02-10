using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.ViewModels;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Desktop_app
{
    /// <summary>
    /// Interaction logic for WorkerManagement.xaml
    /// </summary>
    public partial class WorkerManagement : Window
    {
        private WorkerManagementViewModel vm;
        private double _scaleValue = 1;

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
            vm = viewModel;


            viewModel.DataLoaded += ViewModel_DataLoaded;
            viewModel.ExitFullscreenRequested += OnExitFullscreenRequested;
            Loaded += (_, _) =>
            {
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Loaded,
                    RebuildConnections);
            };
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

            double maxX = vm.Nodes.Any() ? vm.Nodes.Max(n => n.X) + 450 : 800;
            double maxY = vm.Nodes.Any() ? vm.Nodes.Max(n => n.Y) + 100 : 600;

            SubdepartmentCanvas.Width = maxX;
            SubdepartmentCanvas.Height = maxY;

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

        private Button? GetButtonForNode(Node node)
        {
            var presenter = NodeListBox.ItemContainerGenerator
                .ContainerFromItem(node) as ContentPresenter;

            if (presenter == null)
                return null;

            presenter.ApplyTemplate();

            return VisualTreeHelper.GetChild(presenter, 0) as Button;
        }

        private void RebuildConnections()
        {
            vm.Connections.Clear();

            foreach (var parent in vm.Nodes)
            {
                RebuildConnectionsForNode(parent);
            }
        }

        private void RebuildConnectionsForNode(Node parent)
        {
            if (parent.InverseParent == null || parent.InverseParent.Count == 0)
                return;

            var parentButton = GetButtonForNode(parent);
            if (parentButton == null)
                return;

            Point parentTopLeft = parentButton.TransformToAncestor(SubdepartmentCanvas)
                                               .Transform(new Point(0, 0));

            double parentCenterX = parentTopLeft.X + parentButton.ActualWidth / 2;
            double parentBottomY = parentTopLeft.Y + parentButton.ActualHeight;

            foreach (var child in parent.InverseParent)
            {
                var childButton = GetButtonForNode(child);
                if (childButton == null)
                    continue;

                Point childTopLeft = childButton.TransformToAncestor(SubdepartmentCanvas)
                                                .Transform(new Point(0, 0));

                double childCenterX = childTopLeft.X + childButton.ActualWidth / 2;
                double childTopY = childTopLeft.Y;

                // создаём ломаную стрелку через среднюю Y
                double midY = (parentBottomY + childTopY) / 2;

                var geometry = new PathGeometry();
                var figure = new PathFigure { StartPoint = new Point(parentCenterX, parentBottomY) };
                figure.Segments.Add(new LineSegment(new Point(parentCenterX, midY), true));
                figure.Segments.Add(new LineSegment(new Point(childCenterX, midY), true));
                figure.Segments.Add(new LineSegment(new Point(childCenterX, childTopY), true));
                geometry.Figures.Add(figure);

                vm.Connections.Add(new Connection
                {
                    PathGeometry = geometry
                });

                // рекурсивно для детей
                RebuildConnectionsForNode(child);
            }
        }
    }
}