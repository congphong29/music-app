using Microsoft.AspNetCore.Http;

namespace Source.Models.InputModels
{
    public class ProductSearchModel
    {
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}
