using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Source.Models.InputModels
{
    public class ProductInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Singer { get; set; }
        public IFormFile File { get; set; }
    }
}
