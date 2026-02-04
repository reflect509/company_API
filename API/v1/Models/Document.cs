using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class Document
{
    public int DocumentId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly DateApproval { get; set; }

    public DateOnly DateEdit { get; set; }

    public string? Status { get; set; }

    public string DocumentType { get; set; } = null!;

    public string? Field { get; set; }

    public int? AuthorId { get; set; }

    public virtual Worker? Author { get; set; }

    public virtual ICollection<DocumentComment> DocumentComments { get; set; } = new List<DocumentComment>();
}
