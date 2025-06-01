using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Models;

[Table("UserConfiguration")]
public class UserConfiguration
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int CarComponentId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("CarComponentId")]
    public virtual CarComponent CarComponent { get; set; } = null!;
}
