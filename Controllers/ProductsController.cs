using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Turg.App.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        // Returns all products.
        // GET: /products/
        [HttpGet]
        public async Task<List<Product>> Index()
        {
            return await Product.GetAll();
        }

        // Returns products by category name.
        // GET: /products/GetProductsByCategory?category=Outdoors
        [HttpGet("GetProductsByCategory")]
        public async Task<List<Product>> GetProductsByCategory([FromQuery] string category)
        {
            return await Product.GetByCategoryId(category);
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
            var id = await Product.Create(product);
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
