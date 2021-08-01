using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Source.Middleware;
using Source.Models.InputModels;
using Source.Models.ViewModels;
using Source.Services.Interfaces;

namespace Source.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpPost("/search")]
        public PagedResult<ProductModel> Search([FromQuery] int page, [FromQuery] int size, [FromBody] ProductSearchModel filter)
        {
            if (page == 0 && size == 0)
            {
                page = 1;
                size = 99999;
            }

            return _productService.Search(page, size, filter);
        }

        [HttpGet("{id}")]
        public ProductModel GetById(int id)
        {
            return _productService.GetById(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, [FromForm]ProductInputModel Product)
        {
            Product.Id = id;
            _productService.Update(Product);
        }

        [HttpPost]
        public int Create([FromForm]ProductInputModel Product)
        {
            Product.Id = 0;
            return _productService.Create(Product);
        }

        [HttpDelete("{id}")]
        public int Delete(int id)
        {
            return _productService.Delete(id);
        }
    }
}
