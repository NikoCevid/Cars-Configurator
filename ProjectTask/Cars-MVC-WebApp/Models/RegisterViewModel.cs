using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        [Remote(action: "IsUsernameAvailable", controller: "Auth")]
        [Display(Name = "Korisničko ime")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email je obavezan.")]
        [EmailAddress(ErrorMessage = "Neispravan format email adrese.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ime je obavezno.")]
        [Display(Name = "Ime")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [Display(Name = "Prezime")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lozinke se ne podudaraju.")]
        [Display(Name = "Potvrdi lozinku")]
        public string ConfirmPassword { get; set; } = string.Empty;





    }
}
