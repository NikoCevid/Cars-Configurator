using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class LogEntry
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Razina")]
        public string? Level { get; set; }

        [Display(Name = "Poruka")]
        public string? Message { get; set; }

        [Display(Name = "Vrijeme")]
        public DateTime Timestamp { get; set; }
    }
}
