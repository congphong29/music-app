using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Source.Middleware;
using Source.Models;
using Source.Models.InputModels;
using Source.Models.ViewModels;
using Source.Services.Interfaces;

namespace Source.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet]
        public PagedResult<CategoryModel> Search([FromQuery] int page, [FromQuery] int size, string keyword)
        {
            if (page == 0 && size == 0)
            {
                page = 1;
                size = 99999;
            }
            
            return _categoryService.Search(page, size, keyword);
        }

        [HttpGet("{id}")]
        public CategoryModel GetById(int id)
        {
            return _categoryService.GetById(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, [FromForm]CategoryInputModel category)
        {
            category.Id = id;
            _categoryService.Update(category);
        }

        [HttpPost]
        public int Create([FromForm]CategoryInputModel category)
        {
            category.Id = 0;
            return _categoryService.Create(category);
        }

        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return _categoryService.Delete(id);
        }
    }
}
