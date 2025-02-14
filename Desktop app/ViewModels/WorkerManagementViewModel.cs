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
        private readonly IApiService apiService;  
        
        private ObservableCollection<Node> nodes;

        public ObservableCollection<Node> Nodes
        {
            get { return nodes; }
            set 
            { 
                nodes = value;
                OnPropertychanged(nameof(Nodes));
            }
        }

        private ObservableCollection<Line> lines;

        public ObservableCollection<Line> Lines
        {
            get { return lines; }
            set 
            { 
                lines = value;
                OnPropertychanged(nameof(Lines));
            }
        }


        public WorkerManagementViewModel(IApiService apiService)
        {
            this.apiService = apiService;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            var nodes = new ObservableCollection<Node>(await apiService.GetSubdepartmentsAsync());
            foreach (var node in nodes)
            {
                nodes.Add(node);
            }
            
            CalculatePositions();
            DrawLines();
        }

        private void CalculatePositions()
        {
            LayoutNodes(Nodes, 0, 0);
        }

        private void LayoutNodes(ObservableCollection<Node> nodes, int level, double offsetX)
        {
            double horizontalSpacing = 150;
            double verticalSpacing = 100;
            int index = 0;

            foreach (var node in nodes)
            {
                node.Level = level;
                node.Y = level * verticalSpacing;

                node.X = offsetX + index * horizontalSpacing;
                index++;

                if (node.Children != null && node.Children.Count != 0)
                {
                    LayoutNodes(new ObservableCollection<Node>(node.Children), level + 1, node.X);
                }
            }
        }

        private void DrawLines()
        {
            foreach (var node in Nodes)
            {
                DrawLinesForNode(node);
            }
        }

        private void DrawLinesForNode(Node parent)
        {
            if (parent.Children != null)
            {
                foreach (var child in parent.Children)
                {                   
                    double parentCenterX = parent.X + 50; // Assuming node width of 100
                    double parentBottomY = parent.Y + 50;   // Assuming node height of 50
                    double childCenterX = child.X + 50;
                    double childTopY = child.Y;            

                    var line = new Line
                    {
                        X1 = parentCenterX,
                        Y1 = parentBottomY,
                        X2 = childCenterX,
                        Y2= childTopY,
                        Stroke = Brushes.Black,
                        StrokeThickness = 2
                    };

                    Lines.Add(line);

                    DrawLinesForNode(child);
                }
            }
        }



        public void OnPropertychanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
