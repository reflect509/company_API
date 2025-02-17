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


        public WorkerManagementViewModel(IApiService apiService)
        {
            this.apiService = apiService;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var nodes = await apiService.GetSubdepartmentsAsync();
            CalculatePositions(nodes);
            DrawConnections();
            DataLoaded?.Invoke(this, EventArgs.Empty);
        }

        private void CalculatePositions(IEnumerable<Node> nodes)
        {            
            LayoutNodes(nodes.ToList(), level: 0, offsetX: 0);
        }

        private void LayoutNodes(List<Node> nodes, int level, double offsetX)
        {
            double horizontalSpacing = 250;
            double verticalSpacing = 100;
            int index = 0;

            foreach (var node in nodes)
            {
                node.Level = level;
                node.Y = level * verticalSpacing;
                node.X = offsetX + index * horizontalSpacing;               
                index++;

                if (!Nodes.Any(n => n.SubdepartmentName == node.SubdepartmentName))
                {
                    Nodes.Add(node);
                }

                if (node.Children != null && node.Children.Count != 0)
                {
                    LayoutNodes(new List<Node>(node.Children), level: level + 1, offsetX: node.X);
                    index++;
                }
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
                    double parentCenterX = parent.X + 100; // Assuming node width of 100
                    double parentBottomY = parent.Y + 50;   // Assuming node height of 50
                    double childCenterX = child.X + 100;
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



        public void OnPropertychanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
