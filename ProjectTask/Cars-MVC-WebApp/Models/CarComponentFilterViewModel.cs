namespace Cars_MVC.Models
{
    public class CarComponentFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public int? ComponentTypeId { get; set; }
        public int Page { get; set; } = 1;
    }
}
