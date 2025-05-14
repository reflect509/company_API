using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilApp.Models
{
    public class NewsItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int PositiveReactions { get; set; }
        public int NegativeReactions { get; set; }
    }
}
