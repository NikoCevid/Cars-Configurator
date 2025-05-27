using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dao.Models;

[Table("Configuration")]
public partial class Configuration
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreationDate { get; set; }

    [InverseProperty("Configuration")]
    public virtual ICollection<ConfigurationCarComponent> ConfigurationCarComponents { get; set; } = new List<ConfigurationCarComponent>();

    [ForeignKey("UserId")]
    [InverseProperty("Configurations")]
    public virtual User User { get; set; } = null!;
}
