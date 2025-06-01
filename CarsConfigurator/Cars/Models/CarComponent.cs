using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class CarComponent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int ComponentTypeId { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<CarComponentCompatibility> CarComponentCompatibilityCarComponentId1Navigations { get; set; } = new List<CarComponentCompatibility>();

    public virtual ICollection<CarComponentCompatibility> CarComponentCompatibilityCarComponentId2Navigations { get; set; } = new List<CarComponentCompatibility>();

    public virtual ComponentType ComponentType { get; set; } = null!;

    public virtual ICollection<ConfigurationCarComponent> ConfigurationCarComponents { get; set; } = new List<ConfigurationCarComponent>();

    public virtual ICollection<UserConfiguration> UserConfigurations { get; set; } = new List<UserConfiguration>();
}
