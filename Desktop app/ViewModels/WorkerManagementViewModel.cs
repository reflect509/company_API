using CommunityToolkit.Mvvm.Input;
using Desktop_app.Models;
using Desktop_app.Services;
using Desktop_app.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Desktop_app.ViewModels
{
    public class WorkerManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event Action? ExitFullscreenRequested;
        public event EventHandler DataLoaded;
        private readonly IApiService apiService;
        public ICommand NodeClickedCommand { get;}
        private const double NodeWidth = 200;
        private const double NodeMinHeight = 100;
        private const double HorizontalSpacing = 40;
        private const double VerticalSpacing = 400;

         private ObservableCollection<Node> nodes = new ObservableCollection<Node>();

        public ObservableCollection<Node> Nodes
        {
            get { return nodes; }
            set 
            { 
                nodes = value;
                OnPropertychanged(nameof(Nodes));
            }
        }

        private ObservableCollection<Connection> connections = new ObservableCollection<Connection>();

        public ObservableCollection<Connection> Connections
        {
            get { return connections; }
            set 
            { 
                connections = value;
                OnPropertychanged(nameof(Connections));
            }
        }

        private ObservableCollection<Worker> workers = new ObservableCollection<Worker>();

        public ObservableCollection<Worker> Workers
        {
            get { return workers; }
            set
            {
                workers = value;
                OnPropertychanged(nameof(Workers));
            }
        }

        private Worker selectedWorker;

        public Worker SelectedWorker
        {
            get { return selectedWorker; }
            set 
            { 
                selectedWorker = value;
                OnPropertychanged();
                OnWorkerSelected(selectedWorker);
            }
        }



        public WorkerManagementViewModel(IApiService apiService)
        {
            this.apiService = apiService;
            NodeClickedCommand = new RelayCommand<Node>(OnSubdepartmentClicked);
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var nodes = await apiService.GetSubdepartmentsAsync();
            List<Node> rootNodes = new List<Node>();

            foreach (var node in nodes)
            {
                if (!node.ParentId.HasValue)
                {
                    rootNodes.Add(node);
                }
            }

            CalculatePositions(rootNodes);
            DrawConnections();
            DataLoaded?.Invoke(this, EventArgs.Empty);
        }

        private double CalculateSubtreeWidth(Node node)
        {
            double nodeWidth = NodeWidth;
            double horizontalSpacing = HorizontalSpacing;

            if (node.InverseParent == null || node.InverseParent.Count == 0)
            {
                node.SubtreeWidth = nodeWidth;
                return node.SubtreeWidth;
            }

            double childrenWidth = 0;

            foreach (var child in node.InverseParent)
            {
                childrenWidth += CalculateSubtreeWidth(child);
            }

            childrenWidth += horizontalSpacing * (node.InverseParent.Count - 1);

            node.SubtreeWidth = Math.Max(nodeWidth, childrenWidth);
            return node.SubtreeWidth;
        }

        public void CalculatePositions(IEnumerable<Node> roots)
        {
            Nodes.Clear();

            double startX = 10;

            foreach (var root in roots)
            {
                CalculateSubtreeWidth(root);
                LayoutNode(root, startX, 0);
                startX += root.SubtreeWidth + 50;
            }
        }

        private void LayoutNode(Node node, double startX, int level)
        {
            double nodeWidth = NodeWidth;       
            double verticalSpacing = VerticalSpacing;
            double horizontalSpacing = HorizontalSpacing;

            node.Level = level;
            node.Y = level * verticalSpacing + 10;

            // центрируем родителя
            node.X = startX + (node.SubtreeWidth - nodeWidth) / 2;

            Nodes.Add(node);

            if (node.InverseParent == null || node.InverseParent.Count == 0)
                return;

            double childX = startX;

            foreach (var child in node.InverseParent)
            {
                LayoutNode(child, childX, level + 1);
                childX += child.SubtreeWidth + horizontalSpacing;
            }
        }

        private void DrawConnections()
        {
            foreach (var node in Nodes)
            {
                DrawConnectionsForNode(node);
            }            
        }

        private void DrawConnectionsForNode(Node parent)
        {
            if (parent.InverseParent != null)
            {
                foreach (var child in parent.InverseParent)
                {                   
                    double parentCenterX = parent.X + NodeWidth / 2;
                    double parentBottomY = parent.Y + NodeMinHeight;   // Assuming node height of 50
                    double childCenterX = child.X + NodeWidth / 2;
                    double childTopY = child.Y;            

                    var connection = new Connection
                    {
                        X1 = parentCenterX,
                        Y1 = parentBottomY,
                        X2 = childCenterX,
                        Y2 = childTopY
                    };

                    Connections.Add(connection);

                    DrawConnectionsForNode(child);
                }
            }
        }

        public List<Worker> GetWorkers(Node node)
        {
            var workers = new List<Worker>();

            if (node.Workers != null)
            {
                foreach (var worker in node.Workers)
                {
                    worker.SubdepartmentName = node.SubdepartmentName;
                    workers.Add(worker);
                }
            }

            if (node.InverseParent != null)
            {
                foreach(var child in node.InverseParent)
                {
                    workers.AddRange(GetWorkers(child));
                }
            }

            return workers;
        }

        public void OnSubdepartmentClicked(Node selectedNode)
        {
            if (selectedNode == null)
            {
                return;
            }

            List<Worker> workers = GetWorkers(selectedNode);

            Workers.Clear();

            Workers = new ObservableCollection<Worker>(workers);

            ExitFullscreenRequested?.Invoke();
        }

        private void OnWorkerSelected(Worker worker)
        {
            if (worker == null)
            {
                return;
            }

            var workerCardWindow = new WorkerCard(apiService, worker, Workers);
            workerCardWindow.ShowDialog();
        }

        private void OnPropertychanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
