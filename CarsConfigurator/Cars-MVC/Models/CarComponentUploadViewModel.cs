using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cars_MVC.Models
{
    public class CarComponentUploadViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ComponentTypeId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
