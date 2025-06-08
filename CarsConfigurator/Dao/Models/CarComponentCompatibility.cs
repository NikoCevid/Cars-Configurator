using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Models;

[Table("CarComponentCompatibility")]
public partial class CarComponentCompatibility
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Odaberi prvu komponentu.")]
    public int CarComponentId1 { get; set; }

    [Required(ErrorMessage = "Odaberi drugu komponentu.")]
    public int CarComponentId2 { get; set; }

    [ForeignKey("CarComponentId1")]
    [InverseProperty("CarComponentCompatibilityCarComponentId1Navigations")]
    public virtual CarComponent CarComponentId1Navigation { get; set; } = null!;

    [ForeignKey("CarComponentId2")]
    [InverseProperty("CarComponentCompatibilityCarComponentId2Navigations")]
    public virtual CarComponent CarComponentId2Navigation { get; set; } = null!;
}
