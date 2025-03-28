using Npgsql;
using Turg.App.Infrastructure;
using Turg.App.Models;

namespace Turg.App.Persistence;

// A service should:
// * encapsulate its logic
// * be reusable
// * follow SRP of SOLID
// * EDP
// * DI of SOLID (soon)
// * OCP of SOLID (soon)
public class ProductRepository
{
    private readonly SqlCommandExecutor _sqlCommandExecutor;

    public ProductRepository(SqlCommandExecutor sqlCommandExecutor)
    {
        _sqlCommandExecutor = sqlCommandExecutor;
    }

    public async Task<IEnumerable<Product>> Get(string category = null)
    {
        var commandText = "SELECT * FROM products";
        var parameters = new List<NpgsqlParameter>();

        if (!string.IsNullOrWhiteSpace(category))
        {
            commandText += $" WHERE category = @category";
            parameters.Add(new NpgsqlParameter("category", category));
        }

        var products = await _sqlCommandExecutor.ExecuteReaderAsync(commandText, async reader =>
        {
            var products = new List<Product>();

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

            return products;
        }, parameters.ToArray());

        return products;
    }

    public async Task<Guid> Insert(Product product)
    {
        var id = Guid.NewGuid();
        var commandText = "INSERT INTO products (id, name, category, description, price, currency) VALUES (@id, @name, @category, @description, @price, @currency)";

        await _sqlCommandExecutor.ExecuteNonQueryAsync(commandText,
            new NpgsqlParameter("id", id),
            new NpgsqlParameter("name", product.Name),
            new NpgsqlParameter("category", product.Category),
            new NpgsqlParameter("description", product.Description),
            new NpgsqlParameter("price", product.Price),
            new NpgsqlParameter("currency", product.Currency)
        );

        return id;
    }

    public async Task Update(Product product, Guid? id = null)
    {
        var commandText = "UPDATE products SET name = @name, category = @category, description = @description, price = @price, currency = @currency WHERE id = @id";

        await _sqlCommandExecutor.ExecuteNonQueryAsync(commandText,
            new NpgsqlParameter("id", id ?? product.Id),
            new NpgsqlParameter("name", product.Name),
            new NpgsqlParameter("category", product.Category),
            new NpgsqlParameter("description", product.Description),
            new NpgsqlParameter("price", product.Price),
            new NpgsqlParameter("currency", product.Currency)
        );
    }
}
