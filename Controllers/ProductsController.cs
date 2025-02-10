using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Turg.App.Filters;
using Turg.App.Models;

namespace Turg.App.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0")]
    public class ProductsController : BaseApiController
    {
        [MapToApiVersion("1.0")]
        [HttpGet]
        [ServiceFilter<CachingFilter>]
        public async Task<IEnumerable<Product>> Index()
        {
            var products = await Product.GetAll();
            return products;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [ServiceFilter<CachingFilter>]
        public async Task<IEnumerable<Product>> GetProducts([FromQuery] string category)
        {
            if (!String.IsNullOrWhiteSpace(category))
            {
                var productsByCategory = await Product.GetByCategory(category);
                return productsByCategory;
            }

            var products = await Product.GetAll();
            return products;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetProductsByCategory")]
        public async Task<IEnumerable<Product>> GetProductsByCategory([FromQuery] string category)
        {
            var products = await Product.GetByCategory(category);
            return products;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("AddProduct")]
        public async Task<dynamic> AddProduct([FromBody] Product product)
        {
            var id = await Product.Add(product);
            return new { Result = "OK", Message = "Product added", Id = id };
        }

        [MapToApiVersion("2.0")]
        [HttpPost()]
        public async Task<dynamic> Create([FromBody] Product product)
        {
            var id = await Product.Add(product);
            return new { Result = "OK", Message = "Product added", Id = id };
        }

        [MapToApiVersion("1.0")]
        [HttpGet("UpdateProduct")]
        public async Task<dynamic> UpdateProduct([FromBody] Product product)
        {
            await Product.Update(product);
            return new { Result = "OK", Message = "Product updated" };
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public async Task<dynamic> Put([FromRoute]Guid id, [FromBody] Product product)
        {
            await Product.Update(product, id);
            return new { Result = "OK", Message = "Product updated" };
        }
    }
}
