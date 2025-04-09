namespace Cars.DTO
{
    public class CreateCarComponentDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int ComponentTypeId { get; set; }
        public string? ImagePath { get; set; }
    }
}
