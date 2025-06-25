namespace Cars.DTO
{
    public class ComponentCompatibilityDTO
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public List<CompatibleComponentDTO> CompatibleWith { get; set; } = new();
    }
}
