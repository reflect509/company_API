using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class Subdepartment
{
    public int SubdepartmentId { get; set; }

    public string SubdepartmentName { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}