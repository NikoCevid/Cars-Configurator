using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dao.Models;

[Table("ConfigurationCarComponent")]
public partial class ConfigurationCarComponent
{
    [Key]
    public int Id { get; set; }

    public int ConfigurationId { get; set; }

    public int CarComponentId { get; set; }

    [ForeignKey("CarComponentId")]
    [InverseProperty("ConfigurationCarComponents")]
    public virtual CarComponent CarComponent { get; set; } = null!;

    [ForeignKey("ConfigurationId")]
    [InverseProperty("ConfigurationCarComponents")]
    public virtual Configuration Configuration { get; set; } = null!;
}
