using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class ErrorViewModel
    {
        [Display(Name = "ID zahtjeva")]
        public string? RequestId { get; set; }

        [Display(Name = "Prika�i ID zahtjeva")]
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
