namespace Cars.DTO
{
    public class ConfigurationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public List<ConfigurationCarComponentDTO> ConfigurationCarComponents { get; set; } = new List<ConfigurationCarComponentDTO>();
    }
}
