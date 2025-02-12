using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class AppUser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;
}
