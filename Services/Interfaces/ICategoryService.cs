using Source.Middleware;
using Source.Models;
using Source.Models.InputModels;
using Source.Models.ViewModels;

namespace Source.Services.Interfaces
{
    public interface ICategoryService
    {
        PagedResult<CategoryModel> Search(int page, int size, string keyword);
        CategoryModel GetById(int categoryId);
        int Create(CategoryInputModel category);
        void Update(CategoryInputModel category);
        int Delete(int categoryId);
    }
}
