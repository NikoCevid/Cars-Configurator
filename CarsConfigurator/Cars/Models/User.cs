using System;
using System.Collections.Generic;

namespace Cars.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string PwdHash { get; set; } = null!;

    public string PwdSalt { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Configuration> Configurations { get; set; } = new List<Configuration>();

    public virtual ICollection<UserConfiguration> UserConfigurations { get; set; } = new List<UserConfiguration>();
}
