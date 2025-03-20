using Npgsql;
using Turg.App.Infrastructure;
using Turg.App.Models;

namespace Turg.App.Persistence;

internal class ShoppingCartRepository
{
    private readonly SqlCommandExecutor _sqlCommandExecutor;

    public ShoppingCartRepository()
    {
        _sqlCommandExecutor = new SqlCommandExecutor();
    }

    public async Task<ShoppingCart> GetById(string id)
    {
        var commandText = @"
        SELECT sc.id, p.id, p.name, p.category, p.description, p.price, p.currency
        FROM shopping_carts sc
        LEFT OUTER JOIN shopping_cart_items sci ON sc.id = sci.shopping_cart_id
        LEFT OUTER JOIN products p ON sci.product_id = p.id
        WHERE sc.id = @id";

        var shoppingCart = await _sqlCommandExecutor.ExecuteReaderAsync(commandText, async reader =>
        {
            ShoppingCart shoppingCart = null;

            while (await reader.ReadAsync())
            {
                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCart
                    {
                        Id = reader.GetGuid(0).ToString(),
                        Products = new List<Product>()
                    };
                }

                if (!reader.IsDBNull(1))
                {
                    shoppingCart.Products.Add(new Product
                    {
                        Id = reader.GetGuid(1),
                        Name = reader.GetString(2),
                        Category = reader.GetString(3),
                        Description = reader.GetString(4),
                        Price = reader.GetDouble(5),
                        Currency = reader.GetString(6)
                    });
                }
            }

            return shoppingCart;
        }, new NpgsqlParameter("id", new Guid(id)));

        return shoppingCart;
    }

    public async Task AddProduct(ShoppingCartItem shoppingCartItem, Guid? cartId = null)
    {
        var isNewShoppingCart = shoppingCartItem.ShoppingCartId == null;
        var shoppingCartId = cartId ?? shoppingCartItem.ShoppingCartId ?? Guid.NewGuid();

        if (isNewShoppingCart)
        {
            await _sqlCommandExecutor.ExecuteNonQueryAsync("INSERT INTO shopping_carts (id) VALUES (@id)", new NpgsqlParameter("id", shoppingCartId));
        }

        await _sqlCommandExecutor.ExecuteNonQueryAsync("INSERT INTO shopping_cart_items (shopping_cart_id, product_id, quantity) VALUES (@shoppingCartId, @productId, 1)",

        new NpgsqlParameter("shoppingCartId", shoppingCartId),
        new NpgsqlParameter("productId", new Guid(shoppingCartItem.ProductId)));
    }

    public async Task Delete(string shoppingCartId)
    {
        await _sqlCommandExecutor.ExecuteNonQueryAsync("DELETE FROM shopping_cart_items WHERE shopping_cart_id = @shoppingCartId; DELETE FROM shopping_carts WHERE id = @shoppingCartId",
        new NpgsqlParameter("shoppingCartId", new Guid(shoppingCartId)));
    }
}
