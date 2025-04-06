namespace Turg.App.Persistence;

using Turg.App.Models;

public class ShoppingCartRepositoryMock(ILogger<ShoppingCartRepositoryMock> logger) : IShoppingCartRepository
{
    private readonly List<ShoppingCart> _shoppingCarts = new List<ShoppingCart>
    {
        new ShoppingCart
        {
            Id = Guid.NewGuid().ToString(),
            Products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 10.0,
                    Category = "Category 1"
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Product 2",
                    Description = "Description 2",
                    Price = 20.0,
                    Category = "Category 2"
                }
            }
        }
    };

    public async Task<ShoppingCart> GetById(string id)
    {
        logger.LogInformation("Get shopping cart by id from API");

        return _shoppingCarts.FirstOrDefault(cart => cart.Id == id);
    }
    public async Task AddProduct(ShoppingCartItem shoppingCartItem, Guid? cartId = null)
    {
        logger.LogInformation("Add product to shopping cart in API");

        var cart = _shoppingCarts.FirstOrDefault(c => c.Id == cartId.ToString());
        if (cart != null)
        {
            cart.Products.Add(new Product
            {
                Id = new Guid(shoppingCartItem.ProductId),
                Name = "Product " + shoppingCartItem.ProductId,
                Price = 100,
                Category = "Category " + shoppingCartItem.ProductId,
            });
        }
    }
    public async Task Delete(string id)
    {
        logger.LogInformation("Delete shopping cart from API");

        var cart = _shoppingCarts.FirstOrDefault(c => c.Id == id);
        if (cart != null)
        {
            _shoppingCarts.Remove(cart);
        }
    }
}
