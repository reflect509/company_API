using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_app.Models
{
    public class Node
    {
        public int SubdepartmentId { get; set; }
        public string SubdepartmentName { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
        public int Level { get; set; }

        public List<Node> Children { get; set; } = new List<Node>();
    }
}
