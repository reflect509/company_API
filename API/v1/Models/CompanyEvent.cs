using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class CompanyEvent
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public DateTime? Date { get; set; }

    public string? Description { get; set; }

    public int? AuthorId { get; set; }

    public virtual Worker? Author { get; set; }
}
