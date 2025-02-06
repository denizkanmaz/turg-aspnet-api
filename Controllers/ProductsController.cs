using Microsoft.AspNetCore.Mvc;
using Turg.App.Filters;
using Turg.App.Models;
using Turg.App.Pipelines;

namespace Turg.App.Controllers
{
    // [MiddlewareFilter<CustomMiddlewarePipeline>]
    // public abstract class MyBaseController : Controller
    // {}

    [Route("[controller]")]
    [ServiceFilter<BenchmarkFilter>]
    public class ProductsController : ControllerBase // MyBaseController
    {
        private ILogger<ProductsController> _logger;
        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
            
            _logger.LogInformation("ctor");
        }
        // Returns all products.
        // GET: /products/
        [HttpGet]
        [ServiceFilter<CachingFilter>]
        // [TypeFilter<BenchmarkFilter>]
        // [ServiceFilter<BenchmarkFilter>]
        public async Task<IEnumerable<Product>> Index()
        {
            _logger.LogInformation("index");

            var products = await Product.GetAll();
            return products;

            // Sends data always in JSON format
            // return new JsonResult(products);
            // return Json(Product);
        }

        // Returns products by category name.
        // GET: /products/GetProductsByCategory?category=Outdoors
        [MiddlewareFilter<CustomMiddlewarePipeline>]
        [HttpGet("GetProductsByCategory")]
        public async Task<IEnumerable<Product>> GetProductsByCategory([FromQuery] string category)
        {
            var products = await Product.GetByCategory(category);
            return products;
        }

        // Adds a new product.
        // GET: /products/AddProduct
        // {
        //     "name": "A sample product",
        //     "category": "Sample",
        //     "description": "A sample product description",
        //     "price": 100.00,
        //     "currency": "USD"
        // }
        [HttpGet("AddProduct")]
        public async Task<dynamic> AddProduct([FromBody] Product product)
        {
            var id = await Product.Add(product);
            return new { Result = "OK", Message = "Product added", Id = id };
        }

        // Updates a product.
        // GET: /products/UpdateProduct
        // {
        //     "id": "8b2aedf0-c6a8-4a09-a36a-077055a37133",
        //     "name": "Handcrafted Steel Towels (Updated)",
        //     "category": "Outdoors",
        //     "description": "Updated description",
        //     "price": 100.00,
        //     "currency": "USD"
        // }
        [HttpGet("UpdateProduct")]
        public async Task<dynamic> UpdateProduct([FromBody] Product product)
        {
            await Product.Update(product);
            return new { Result = "OK", Message = "Product updated" };
        }
    }
}
