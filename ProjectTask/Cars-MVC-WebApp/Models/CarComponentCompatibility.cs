using Dao.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class CarComponentCompatibility
    {
        [Required(ErrorMessage = "Odaberi prvu komponentu.")]
        [Display(Name = "Komponenta 1")]
        public int CarComponentId1 { get; set; }

        [Required(ErrorMessage = "Odaberi drugu komponentu.")]
        [Display(Name = "Komponenta 2")]
        public int CarComponentId2 { get; set; }

        [ValidateNever]
        public virtual CarComponent? CarComponentId1Navigation { get; set; }

        [ValidateNever]
        public virtual CarComponent? CarComponentId2Navigation { get; set; }
    }
}
