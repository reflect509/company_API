using Desktop_app.Models;
using Desktop_app.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Desktop_app.ViewModels
{
    public class WorkerManagementViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler DataLoaded;
        private readonly IApiService apiService;  
        
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


        public WorkerManagementViewModel(IApiService apiService)
        {
            this.apiService = apiService;
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

        private void CalculatePositions(IEnumerable<Node> nodes)
        {            
            LayoutNodes(nodes.ToList(), level: 0, offsetX: 0);
        }

        private void LayoutNodes(List<Node> nodes, int level, double offsetX)
        {
            double horizontalSpacing = 425;
            double verticalSpacing = 150;
            int index = 0;

            foreach (var node in nodes)
            {
                if (Nodes.Any(n => n.SubdepartmentName == node.SubdepartmentName))
                {
                    continue;
                }
                node.Level = level;
                node.Y = level * verticalSpacing;
                node.X = offsetX + index * horizontalSpacing;               
                index++;

                

                if (node.Children != null && node.Children.Count != 0)
                {
                    LayoutNodes(new List<Node>(node.Children), level: level + 1, offsetX: node.X);
                    index = 1;
                    offsetX = Nodes.Max(n => n.X);
                }

                Nodes.Add(node);
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
            if (parent.Children != null)
            {
                foreach (var child in parent.Children)
                {                   
                    double parentCenterX = parent.X + 200; // Assuming node width of 400
                    double parentBottomY = parent.Y + 50;   // Assuming node height of 50
                    double childCenterX = child.X + 200;
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
                workers.AddRange(node.Workers);
            }

            if (node.Children != null)
            {
                foreach(var child in node.Children)
                {
                    workers.AddRange(GetWorkers(child));
                }
            }

            return workers;
        }

        public void OnSubdepartmentClicked(Node selectedNode)
        {
            var workers = GetWorkers(selectedNode);

            Workers.Clear();

            foreach (Worker worker in workers)
            {
                Workers.Add(worker);
            }
        }

        public void OnPropertychanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
