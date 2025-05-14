using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class News
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public int? PositiveReactions { get; set; }

    public int? NegativeReactions { get; set; }
}
