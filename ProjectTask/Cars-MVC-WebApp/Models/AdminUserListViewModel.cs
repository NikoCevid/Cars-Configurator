using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class AdminUserListViewModel
    {
        [Display(Name = "Korisnici")]
        public List<Dao.Models.User> Users { get; set; } = new();

        [Display(Name = "Korisničko ime")]
        public string? SearchUsername { get; set; }

        [Display(Name = "Uloga")]
        public string? SearchRole { get; set; }

        [Display(Name = "Dostupne uloge")]
        public List<string> AvailableRoles { get; set; } = new();

        [Display(Name = "Trenutna stranica")]
        public int CurrentPage { get; set; }

        [Display(Name = "Ukupno stranica")]
        public int TotalPages { get; set; }

        [Display(Name = "Veličina stranice")]
        public int PageSize { get; set; }

        [Display(Name = "Opcije veličine stranice")]
        public List<int> PageSizeOptions { get; set; } = new List<int> { 5, 10, 20 };
    }
}
