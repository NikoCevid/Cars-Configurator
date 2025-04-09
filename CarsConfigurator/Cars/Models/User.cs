using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Cars.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(256)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(256)]
    public string PwdHash { get; set; } = null!;

    [StringLength(256)]
    public string PwdSalt { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();
}
