using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Role { get; set; } = null!;
}
