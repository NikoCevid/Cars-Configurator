using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cars.Models;

[Table("CarComponentCompatibility")]
public partial class CarComponentCompatibility
{
    [Key]
    public int Id { get; set; }

    public int CarComponentId1 { get; set; }

    public int CarComponentId2 { get; set; }

    [ForeignKey("CarComponentId1")]
    [InverseProperty("CarComponentCompatibilityCarComponentId1Navigations")]
    public virtual CarComponent CarComponentId1Navigation { get; set; } = null!;

    [ForeignKey("CarComponentId2")]
    [InverseProperty("CarComponentCompatibilityCarComponentId2Navigations")]
    public virtual CarComponent CarComponentId2Navigation { get; set; } = null!;
}
