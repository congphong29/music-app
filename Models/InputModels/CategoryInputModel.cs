using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Source.Models.InputModels
{
    public class CategoryInputModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
