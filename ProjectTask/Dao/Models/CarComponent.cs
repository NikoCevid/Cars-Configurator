﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dao.Models;

[Table("CarComponent")]
public partial class CarComponent
{
    [Key]
    public int Id { get; set; }

    [StringLength(256)]
    public string Name { get; set; } = null!;

    [StringLength(1024)]
    public string? Description { get; set; }

    public int ComponentTypeId { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? ImageBase64 { get; set; }



    [InverseProperty("CarComponentId1Navigation")]
    public virtual ICollection<CarComponentCompatibility> CarComponentCompatibilityCarComponentId1Navigations { get; set; } = new List<CarComponentCompatibility>();

    [InverseProperty("CarComponentId2Navigation")]
    public virtual ICollection<CarComponentCompatibility> CarComponentCompatibilityCarComponentId2Navigations { get; set; } = new List<CarComponentCompatibility>();

    [ForeignKey("ComponentTypeId")]
    [InverseProperty("CarComponents")]
    public virtual ComponentType ComponentType { get; set; } = null!;

    [InverseProperty("CarComponent")]
    public virtual ICollection<ConfigurationCarComponent> ConfigurationCarComponents { get; set; } = new List<ConfigurationCarComponent>();

    [InverseProperty("CarComponent")]
    public virtual ICollection<UserConfiguration> UserConfigurations { get; set; } = new List<UserConfiguration>();

}
