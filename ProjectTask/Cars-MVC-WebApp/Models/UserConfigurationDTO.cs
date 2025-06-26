using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class UserConfigurationDTO
    {
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Odabrane komponente")]
        public List<string> ComponentNames { get; set; } = new();

        [Display(Name = "Datum zadnje konfiguracije")]
        public DateTime? LastConfigurationDate { get; set; }
    }
}
