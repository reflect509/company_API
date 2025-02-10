using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class DocumentComment
{
    public int CommentId { get; set; }

    public int DocumentId { get; set; }

    public string Text { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public int AuthorId { get; set; }

    public virtual Worker Author { get; set; } = null!;

    public virtual Document? Document { get; set; }
}
