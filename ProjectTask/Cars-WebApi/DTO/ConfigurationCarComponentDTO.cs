//namespace Cars.DTO
//{
//    public class ConfigurationCarComponentDTO
//    {
//        public int Id { get; set; }
//        public int CarComponentId { get; set; }
//        public string CarComponentName { get; set; } = null!;
//    }
//}

public class ConfigurationCarComponentDTO
{
    public int CarComponentId { get; set; }
    public string CarComponentName { get; set; } = string.Empty;
}

