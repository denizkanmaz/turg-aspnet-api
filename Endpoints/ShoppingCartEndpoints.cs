using Turg.App.Models;

namespace Turg.App.Endpoints;

internal class ShoppingCartEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        var shoppingCartGroup = routeBuilder.MapGroup("/shoppingcarts");
        shoppingCartGroup.MapGet("/{id}", async (string id) =>
        {
            var shoppingCart = await ShoppingCart.GetById(id);

            if (shoppingCart == null)
            {
                // return NotFound();
                return null;
            }

            return shoppingCart;
        });

        shoppingCartGroup.MapPost("/{id}/items", async (Guid id, ShoppingCartItem shoppingCartItem) =>
        {
            await ShoppingCart.AddProduct(shoppingCartItem, id);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        });

        shoppingCartGroup.MapDelete("/{id}", async (string id) =>
        {
            await ShoppingCart.Delete(id);
        });
    }
}
