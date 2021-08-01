using System;
using System.ComponentModel.DataAnnotations;

namespace Source.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Singer { get; set; }
        public string ImageUrl { get; set; }
        public string FileUrl { get; set; }
        public string Type { get; set; }
        public long FileSize { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
