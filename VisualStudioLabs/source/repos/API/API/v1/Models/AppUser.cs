using System;
using System.Collections.Generic;

namespace API.v1.Models;

public partial class AppUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Password { get; set; }
}
