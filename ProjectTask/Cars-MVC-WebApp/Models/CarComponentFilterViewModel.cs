using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class CarComponentFilterViewModel
    {
        [Display(Name = "Pretraga po nazivu")]
        public string? SearchTerm { get; set; }

        [Display(Name = "Vrsta komponente")]
        public int? ComponentTypeId { get; set; }

        [Display(Name = "Stranica")]
        public int Page { get; set; } = 1;
    }
}
