using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dao.Models;

[Table("User")]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Email je obavezan.")]
    [EmailAddress(ErrorMessage = "Neispravan format email adrese.")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Telefon je obavezan.")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Required]
    [StringLength(256)]
    public string PwdHash { get; set; } = null!;

    [Required]
    [StringLength(256)]
    public string PwdSalt { get; set; } = null!;

    [Required(ErrorMessage = "Korisničko ime je obavezno.")]
    [StringLength(100)]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Uloga je obavezna.")]
    [StringLength(100)]
    public string Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();

    [InverseProperty("User")]
    public virtual ICollection<UserConfiguration> UserConfigurations { get; set; } = new List<UserConfiguration>();
}
