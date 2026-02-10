using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Desktop_app.Models
{
    public class Connection
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        public PathGeometry PathGeometry { get; set; } = null!;
    }
}
