using System.Diagnostics;
using Turg.App.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Turg.App.Persistence;

namespace Turg.App.Endpoints;

internal class ShoppingCartEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        var shoppingCartGroup = routeBuilder
        .MapGroup("/shoppingcarts")
        .MapToApiVersion(3, 0);

        shoppingCartGroup.MapGet("/{id}", GetById);
        shoppingCartGroup.MapPost("/{id}/items", AddItems).WithValidation<ShoppingCartItem>();
        shoppingCartGroup.MapDelete("/{id}", async (string id) => await new ShoppingCartRepository().Delete(id));
    }

    private static async Task<Results<ProblemHttpResult, Ok<ShoppingCart>>> GetById(HttpContext context, string id)
    {
        var shoppingCartRepository = new ShoppingCartRepository();

        var shoppingCart = await shoppingCartRepository.GetById(id);

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

    private static async Task<Ok<object>> AddItems(Guid id, ShoppingCartItem shoppingCartItem)
    {
        var shoppingCartRepository = new ShoppingCartRepository();

        await shoppingCartRepository.AddProduct(shoppingCartItem, id);
        return TypedResults.Ok<object>(new { Result = "OK", Message = "Product added to shopping cart" });
    }
}
