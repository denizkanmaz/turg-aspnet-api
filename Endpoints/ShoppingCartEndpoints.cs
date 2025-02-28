using System.Diagnostics;
using Turg.App.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Turg.App.Endpoints;

internal class ShoppingCartEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        var shoppingCartGroup = routeBuilder.MapGroup("/shoppingcarts");
        shoppingCartGroup.MapGet("/{id}", GetById);

        shoppingCartGroup.MapPost("/{id}/items", async (Guid id, ShoppingCartItem shoppingCartItem) =>
        {
            await ShoppingCart.AddProduct(shoppingCartItem, id);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        }).AddEndpointFilter<ValidationEndpointFilter<ShoppingCartItem>>();

        shoppingCartGroup.MapDelete("/{id}", async (string id) =>
        {
            await ShoppingCart.Delete(id);
        });
    }

    public static async Task<Results<ProblemHttpResult, Ok<ShoppingCart>>> GetById(HttpContext context, string id)
    {
        var shoppingCart = await ShoppingCart.GetById(id);

        if (shoppingCart == null)
        {
            return TypedResults.Problem(
                title: "Not Found",
                statusCode: 404,
                extensions: new Dictionary<string, object>{
                        {
                            "traceId", Activity.Current?.Id ?? context.TraceIdentifier
                        },
                }
            );
        }

        return TypedResults.Ok(shoppingCart);
    }
}
