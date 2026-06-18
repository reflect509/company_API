using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.v1.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? Date { get; set; }

    public string? Description { get; set; }

    [JsonIgnore]
    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
