namespace Cars.DTO
{
    public class ConfigurationCarComponentDTO
    {
        public int Id { get; set; }
        public int ConfigurationId { get; set; }
        public int CarComponentId { get; set; }

        public string? CarComponentName { get; set; } 

    }
}
