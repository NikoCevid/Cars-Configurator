namespace Cars_MVC.Models
{
    public class AdminUserListViewModel
    {
        public List<Dao.Models.User> Users { get; set; } = new();
        public string? SearchUsername { get; set; }
        public string? SearchRole { get; set; }
        public List<string> AvailableRoles { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
