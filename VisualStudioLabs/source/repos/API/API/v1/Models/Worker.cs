using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? Birthdate { get; set; }

    public string? Phone { get; set; }

    public string Office { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool? IsSubdepartmentHead { get; set; }

    public string JobPosition { get; set; } = null!;

    public string SubdepartmentName { get; set; } = null!;

    public virtual ICollection<DocumentComment> DocumentComments { get; set; } = new List<DocumentComment>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Subdepartment SubdepartmentNameNavigation { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
