using System;
using System.Collections.Generic;

namespace Source.Models.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

        public List<ProductModel> Products { get; set; }

        public CategoryModel(Category entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Description = entity.Description;
            ImageUrl = entity.ImageUrl;
            CreateDate = entity.CreateDate;
            ModifyDate = entity.ModifyDate;
        }
    }
}
