using Microsoft.AspNetCore.Mvc;
using Turg.App.Models;
using Turg.App.Persistence;

namespace Turg.App.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0", Deprecated = true)]
    public class ProductsController : BaseApiController
    {
        private readonly ProductRepository _productRepository;

        public ProductsController(IServiceProvider services)
        {
            Console.WriteLine("ProductsController:ctor");
            _productRepository = services.GetRequiredService<ProductRepository>();
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [ServiceFilter<CachingFilter>]
        public async Task<IEnumerable<Product>> Index()
        {
            var products = await _productRepository.Get();
            return products;
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [ServiceFilter<CachingFilter>]
        public async Task<IEnumerable<Product>> GetProducts([FromQuery] string category)
        {
            var products = await _productRepository.Get(category);
            return products;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetProductsByCategory")]
        public async Task<IEnumerable<Product>> GetProductsByCategory([FromQuery] string category)
        {
            var products = await _productRepository.Get(category);
            return products;
        }

        [MapToApiVersion("1.0")]
        [HttpGet("AddProduct")]
        public async Task<dynamic> AddProduct([FromBody] Product product)
        {
            var id = await _productRepository.Insert(product);
            return new { Result = "OK", Message = "Product added", Id = id };
        }

        [MapToApiVersion("2.0")]
        [HttpPost()]
        public async Task<dynamic> Create([FromBody] Product product)
        {
            var id = await _productRepository.Insert(product);
            return new { Result = "OK", Message = "Product added", Id = id };
        }

        [MapToApiVersion("1.0")]
        [HttpGet("UpdateProduct")]
        public async Task<dynamic> UpdateProduct([FromBody] Product product)
        {
            await _productRepository.Update(product);
            return new { Result = "OK", Message = "Product updated" };
        }

        [MapToApiVersion("2.0")]
        [HttpPut("{id}")]
        public async Task<dynamic> Put([FromRoute] Guid id, [FromBody] Product product)
        {
            await _productRepository.Update(product, id);
            return new { Result = "OK", Message = "Product updated" };
        }
    }
}
