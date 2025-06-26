using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class CarComponentUploadViewModel
    {
        [Required(ErrorMessage = "Naziv je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 znakova.")]
        [Display(Name = "Naziv komponente")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis")]
        [StringLength(1024, ErrorMessage = "Opis može imati najviše 1024 znaka.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Odaberi tip komponente.")]
        [Display(Name = "Tip komponente")]
        public int ComponentTypeId { get; set; }

        [Display(Name = "Slika (upload)")]
        public IFormFile? Image { get; set; }

        [Display(Name = "Slika (Base64)")]
        public string? ImageBase64 { get; set; }
    }
}
