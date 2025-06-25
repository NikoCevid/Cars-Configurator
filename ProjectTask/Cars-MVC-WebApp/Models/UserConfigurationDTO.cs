namespace Cars_MVC.Models
{
    public class UserConfigurationDTO
    {
        public string Username { get; set; } = string.Empty;
        public List<string> ComponentNames { get; set; } = new();

        public DateTime? LastConfigurationDate { get; set; }
    }

}
