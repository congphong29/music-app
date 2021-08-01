using System;
using System.IO;
using System.Linq;
using Source.Middleware;
using Source.Models;
using Source.Models.InputModels;
using Source.Models.ViewModels;
using Source.Services.Interfaces;

namespace Source.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        public CategoryService(ApplicationDbContext context, IProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public PagedResult<CategoryModel> Search(int page, int size, string keyword)
        {
            var result = new PagedResult<CategoryModel> { Page = page, Size = size };
            var skipRow = DataPagerExtension.GetSkipRow(page, size);
            var query = _context.Category.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            result.Total = query.LongCount();

            result.Data = query
                        .OrderBy(x => x.CreateDate)
                        .Skip(skipRow)
                        .Take(size)
                        .Select(x => new CategoryModel(x)).ToList();

            return result;
        }

        public CategoryModel GetById(int categoryId)
        {
            var entity = _context.Category.FirstOrDefault(x => x.Id == categoryId);
            if (entity == null)
            {
                throw new NotFoundException("Id not found");
            }

            var result = new CategoryModel(entity);
            result.Products = _productService.GetByCategoryId(entity.Id);

            return result;
        }

        public int Create(CategoryInputModel category)
        {
            var currentDate = DateTime.UtcNow;
            
            var entity = new Category();
            entity.ImageUrl = FileUploadHelper.UploadThumbnailImage(category.File);
            entity.Name = category.Name;
            entity.Description = category.Description;
            entity.CreateDate = currentDate;
            entity.ModifyDate = currentDate;

            _context.Category.Add(entity);
            _context.SaveChanges();

            return entity.Id;
        }

        public void Update(CategoryInputModel category)
        {
            var entity = _context.Category.FirstOrDefault(x => x.Id == category.Id);
            if (entity == null) return;

            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                // Delete existed one
                FileInfo file = new FileInfo(entity.ImageUrl);
                if (file.Exists)
                    File.Delete(entity.ImageUrl);
            }

            entity.ImageUrl = FileUploadHelper.UploadThumbnailImage(category.File);
            entity.Name = category.Name;
            entity.Description = category.Description;
            entity.ModifyDate = DateTime.UtcNow;

            _context.Category.Update(entity);
            _context.SaveChanges();
        }

        public int Delete(int categoryId)
        {
            var entity = _context.Category.FirstOrDefault(x => x.Id == categoryId);
            if (entity != null)
            {
                var products = _context.Product.Where(x => x.CategoryId == categoryId).ToList();
                var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Category.Remove(entity);
                    if (products.Any())
                        _context.Product.RemoveRange(products);
                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

                // Delete files
                products.ForEach(x =>
                {
                    if (!string.IsNullOrEmpty(x.ImageUrl))
                    {
                        FileInfo image = new FileInfo(x.ImageUrl);
                        if (image.Exists)
                        File.Delete(x.ImageUrl);
                    }
                    
                    if (!string.IsNullOrEmpty(x.FileUrl))
                    {
                        FileInfo file = new FileInfo(x.FileUrl);
                        if (file.Exists)
                        File.Delete(x.FileUrl);
                    }
                });

                if (!string.IsNullOrEmpty(entity.ImageUrl))
                {
                    // Delete existed one
                    FileInfo file = new FileInfo(entity.ImageUrl);
                    if (file.Exists)
                        File.Delete(entity.ImageUrl);
                }
            }

            return categoryId;
        }
    }
}
