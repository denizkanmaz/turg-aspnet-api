using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Turg.App.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public async Task<List<Product>> Index()
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();

            var products = new List<Product>();

            await using (var cmd = new NpgsqlCommand("SELECT * FROM products", conn))
            {
                await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Category = reader.GetString(2),
                        Description = reader.GetString(3),
                        Price = reader.GetDouble(4),
                        Currency = reader.GetString(5)
                    });
                }
            }

            await conn.CloseAsync();
            return products;
        }

        [HttpGet("GetProductsByCategory")]
        public async Task<List<Product>> GetProductsByCategory([FromQuery] string category)
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();

            var products = new List<Product>();

            await using (var cmd = new NpgsqlCommand($"SELECT * FROM products WHERE category = '{category}'", conn))
            {
                await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Category = reader.GetString(2),
                        Description = reader.GetString(3),
                        Price = reader.GetDouble(4),
                        Currency = reader.GetString(5)
                    });
                }
            }

            await conn.CloseAsync();
            return products;
        }

        [HttpGet("AddProduct")]
        public async Task<dynamic> AddProduct([FromBody] Product product)
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();

            var id = Guid.NewGuid();

            await using (var cmd = new NpgsqlCommand("INSERT INTO products (id, name, category, description, price, currency) VALUES (@id, @name, @category, @description, @price, @currency)", conn))
            {
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", product.Name);
                cmd.Parameters.AddWithValue("category", product.Category);
                cmd.Parameters.AddWithValue("description", product.Description);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("currency", product.Currency);

                await cmd.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();
            return new { Result = "OK", Message = "Product added", Id = id };
        }

        [HttpGet("UpdateProduct")]
        public async Task<dynamic> UpdateProduct([FromBody] Product product)
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand("UPDATE products SET name = @name, category = @category, description = @description, price = @price, currency = @currency WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", product.Id);
                cmd.Parameters.AddWithValue("name", product.Name);
                cmd.Parameters.AddWithValue("category", product.Category);
                cmd.Parameters.AddWithValue("description", product.Description);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("currency", product.Currency);

                await cmd.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();
            return new { Result = "OK", Message = "Product updated" };
        }
    }
}
