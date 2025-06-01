using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class ConfigurationCarComponent
{
    public int Id { get; set; }

    public int ConfigurationId { get; set; }

    public int CarComponentId { get; set; }

    public virtual CarComponent CarComponent { get; set; } = null!;

    public virtual Configuration Configuration { get; set; } = null!;
}
