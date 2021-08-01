using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Source.Middleware;
using Source.Models;
using Source.Models.InputModels;
using Source.Models.ViewModels;
using Source.Services.Interfaces;

namespace Source.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public PagedResult<ProductModel> Search(int page, int size, ProductSearchModel filter)
        {
            var result = new PagedResult<ProductModel> { Page = page, Size = size };
            var skipRow = DataPagerExtension.GetSkipRow(page, size);
            var query = _context.Product.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(x => x.Name.Contains(filter.Keyword));
            }
            
            if (filter.CategoryId != null)
            {
                query = query.Where(x => x.CategoryId == filter.CategoryId.Value);
            }

            result.Total = query.LongCount();

            result.Data = query
                        .OrderBy(x => x.CreateDate)
                        .Skip(skipRow)
                        .Take(size)
                        .Select(x => new ProductModel(x)).ToList();

            return result;
        }
        
        public List<ProductModel> GetByCategoryId(int categoryId)
        {
            // get 10 by default
            return _context.Product.Where(x => x.CategoryId == categoryId).OrderBy(x => x.CreateDate).Take(10).Select(x => new ProductModel(x)).ToList();
        }

        public ProductModel GetById(int productId)
        {
            var entity = _context.Product.FirstOrDefault(x => x.Id == productId);
            if (entity == null) throw new NotFoundException();
            return new ProductModel(entity);
        }

        [Obsolete]
        public int Create(ProductInputModel product)
        {
            var category = _context.Category.FirstOrDefault(x => x.Id == product.CategoryId);
            if (category == null) throw new NotFoundException("Invalid CategoryId");

            var currentDate = DateTime.UtcNow;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var entity = new Product();
            if (product.Image != null) 
                entity.ImageUrl = FileUploadHelper.UploadImage(product.Image);
            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.Singer = product.Singer;
            entity.CategoryId = category.Id;
            entity.FileUrl = FileUploadHelper.UploadFile(product.File);
            entity.FileSize = product.File.Length;
            entity.Description = product.Description;
            entity.Type = product.File.ContentType;

            var tagLibFile = TagLib.File.Create(entity.FileUrl);
            entity.Duration = tagLibFile.Properties.Duration.TotalSeconds;

            entity.ModifyDate = currentDate;
            entity.CreateDate = currentDate;

            _context.Product.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public void Update(ProductInputModel product)
        {
            var entity = _context.Product.FirstOrDefault(x => x.Id == product.Id);
            if (entity == null) return;

            var category = _context.Category.FirstOrDefault(x => x.Id == product.CategoryId);
            if (category == null) throw new NotFoundException("Invalid CategoryId");

            // Delete existed files
            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                FileInfo image = new FileInfo(entity.ImageUrl);
                if (image.Exists)
                    System.IO.File.Delete(entity.ImageUrl);
            }
                
            if(!string.IsNullOrEmpty(entity.FileUrl))
            {
                FileInfo file = new FileInfo(entity.FileUrl);
                if (file.Exists)
                    System.IO.File.Delete(entity.FileUrl);
            }
            
            if (product.Image != null) 
                entity.ImageUrl = FileUploadHelper.UploadImage(product.Image);

            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.Singer = product.Singer;
            entity.CategoryId = category.Id;
            entity.FileUrl = FileUploadHelper.UploadFile(product.File);
            entity.FileSize = product.File.Length;
            entity.Description = product.Description;
            entity.Type = product.File.ContentType;

            var tagLibFile = TagLib.File.Create(entity.FileUrl);
            entity.Duration = tagLibFile.Properties.Duration.TotalSeconds;

            entity.ModifyDate = DateTime.UtcNow;

            _context.Product.Update(entity);
            _context.SaveChanges();
        }

        public int Delete(int productId)
        {
            var entity = _context.Product.FirstOrDefault(x => x.Id == productId);
            if (entity != null)
            {
                _context.Product.Remove(entity);
                _context.SaveChanges();
            }
            
            // Delete existed files
            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                FileInfo image = new FileInfo(entity.ImageUrl);
                if (image.Exists)
                    System.IO.File.Delete(entity.ImageUrl);
            }
                
            if(!string.IsNullOrEmpty(entity.FileUrl))
            {
                FileInfo file = new FileInfo(entity.FileUrl);
                if (file.Exists)
                    System.IO.File.Delete(entity.FileUrl);
            }

            return productId;
        }
    }
}
