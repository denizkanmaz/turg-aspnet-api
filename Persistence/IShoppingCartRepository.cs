namespace Turg.App.Persistence;

using Turg.App.Models;

public interface IShoppingCartRepository
{
    Task<ShoppingCart> GetById(string id);
    Task AddProduct(ShoppingCartItem shoppingCartItem, Guid? id = null);
    Task Delete(string id);
}