using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class CarComponentCompatibility
{
    public int Id { get; set; }

    public int CarComponentId1 { get; set; }

    public int CarComponentId2 { get; set; }

    public virtual CarComponent CarComponentId1Navigation { get; set; } = null!;

    public virtual CarComponent CarComponentId2Navigation { get; set; } = null!;
}
