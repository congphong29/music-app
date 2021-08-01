using System;

namespace Source.Models.ViewModels
{
    public class ProductModel
    {
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

        public ProductModel(Product entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            Duration = entity.Duration;
            CategoryId = entity.CategoryId;
            Singer = entity.Singer;
            ImageUrl = entity.ImageUrl;
            FileUrl = entity.FileUrl;
            FileSize = entity.FileSize;
            Type = entity.Type;
            CreateDate = entity.CreateDate;
            ModifyDate = entity.ModifyDate;
        }
    }
}
