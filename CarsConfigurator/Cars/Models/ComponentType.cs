using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cars.Models;

[Table("ComponentType")]
public partial class ComponentType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("ComponentType")]
    public virtual ICollection<CarComponent> CarComponents { get; set; } = new List<CarComponent>();
}
