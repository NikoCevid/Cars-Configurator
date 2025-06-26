using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; } = string.Empty;
    }
}
