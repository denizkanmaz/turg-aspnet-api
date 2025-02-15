using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Npgsql;

namespace Turg.App.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        public static async Task<IEnumerable<Product>> GetAll()
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

            return products.AsReadOnly();
        }

        public static async Task<IEnumerable<Product>> GetByCategory(string category)
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

            return products.AsReadOnly(); ;
        }

        public static async Task<Guid> Add(Product product)
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

            return id;
        }

        public static async Task Update(Product product, Guid? id = null)
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();

            await using (var cmd = new NpgsqlCommand("UPDATE products SET name = @name, category = @category, description = @description, price = @price, currency = @currency WHERE id = @id", conn))
            {
                cmd.Parameters.AddWithValue("id", id ?? product.Id);
                cmd.Parameters.AddWithValue("name", product.Name);
                cmd.Parameters.AddWithValue("category", product.Category);
                cmd.Parameters.AddWithValue("description", product.Description);
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("currency", product.Currency);

                await cmd.ExecuteNonQueryAsync();
            }

            await conn.CloseAsync();
        }
    }
}
