using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class UserConfiguration
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CarComponentId { get; set; }

    public virtual CarComponent CarComponent { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
