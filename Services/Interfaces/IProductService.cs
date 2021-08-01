using System.Collections.Generic;
using Source.Middleware;
using Source.Models;
using Source.Models.InputModels;
using Source.Models.ViewModels;

namespace Source.Services.Interfaces
{
    public interface IProductService
    {
        PagedResult<ProductModel> Search(int page, int size, ProductSearchModel filter);
        List<ProductModel> GetByCategoryId(int categoryId);
        ProductModel GetById(int productId);
        int Create(ProductInputModel product);
        void Update(ProductInputModel product);
        int Delete(int productId);
    }
}
