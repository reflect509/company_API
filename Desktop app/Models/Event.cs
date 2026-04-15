using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_app.Models
{
    public class Event
    {
        public int EventId { get; set; }

        public string EventName { get; set; } = null!;

        public string EventType { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime? Date { get; set; }

        public string? Description { get; set; }
    }
}
