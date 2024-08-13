using System.Text.Json.Serialization;
using Npgsql;

namespace Turg.App
{
    public class ShoppingCart
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("totalPrice")]
        public double TotalPrice { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        public static async Task<ShoppingCart> GetById(string id)
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();

            var shoppingCart = new ShoppingCart();

            await using (var cmd = new NpgsqlCommand($"SELECT * FROM shopping_carts where id = '{id}'", conn))
            {
                await using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    shoppingCart.Id = reader.GetGuid(0).ToString();
                    shoppingCart.Products = new List<Product>();

                    var conn2 = new NpgsqlConnection(Constants.ConnectionString);
                    conn2.Open();

                    var cmd2 = new NpgsqlCommand($"SELECT p.* FROM shopping_cart_items as sci INNER JOIN products as p on p.id = sci.product_id WHERE sci.shopping_cart_id = '{shoppingCart.Id}'", conn2);
                    await using NpgsqlDataReader reader2 = await cmd2.ExecuteReaderAsync();

                    while (await reader2.ReadAsync())
                    {
                        shoppingCart.Products.Add(new Product
                        {
                            Id = reader2.GetGuid(0),
                            Name = reader2.GetString(1),
                            Category = reader2.GetString(2),
                            Description = reader2.GetString(3),
                            Price = reader2.GetDouble(4),
                            Currency = reader2.GetString(5)
                        });
                    }

                    cmd2.Dispose();
                }
            }

            await conn.CloseAsync();

            return shoppingCart;
        }

        public static async Task AddProduct(ShoppingCartItem shoppingCartItem)
        {
            var isNewShoppingCart = shoppingCartItem.ShoppingCartId == null;

            var shoppingCartId = shoppingCartItem.ShoppingCartId ?? Guid.NewGuid();

            if (isNewShoppingCart)
            {
                await using var conn = new NpgsqlConnection(Constants.ConnectionString);
                await conn.OpenAsync();
                var cmd = new NpgsqlCommand($"INSERT INTO shopping_carts (id) VALUES ('{shoppingCartId}')", conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
            }

            await using var conn2 = new NpgsqlConnection(Constants.ConnectionString);
            await conn2.OpenAsync();
            var cmd2 = new NpgsqlCommand($"INSERT INTO shopping_cart_items (shopping_cart_id, product_id, quantity) VALUES ('{shoppingCartId}', '{shoppingCartItem.ProductId}', {1})", conn2);
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();
            conn2.Close();
        }

        public static async Task Delete(string id)
        {
            await using var conn = new NpgsqlConnection(Constants.ConnectionString);
            await conn.OpenAsync();
            var cmd = new NpgsqlCommand($"DELETE FROM shopping_cart_items where shopping_cart_id = '{id}'; DELETE FROM shopping_carts WHERE id = '{id}'", conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }
    }
}
