using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class Configuration
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<ConfigurationCarComponent> ConfigurationCarComponents { get; set; } = new List<ConfigurationCarComponent>();

    public virtual User User { get; set; } = null!;
}
