using System.Diagnostics;
using Turg.App.Models;

namespace Turg.App.Endpoints;

internal class ShoppingCartEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        var shoppingCartGroup = routeBuilder.MapGroup("/shoppingcarts");
        shoppingCartGroup.MapGet("/{id}", async (HttpContext context, string id) =>
        {
            var shoppingCart = await ShoppingCart.GetById(id);

            if (shoppingCart == null)
            {
                // MVC: IActionResult
                // Minimal API: IResult

                // return Results.NotFound();
                return Results.Problem(
                    title: "Not Found",
                    statusCode: 404,
                    extensions: new Dictionary<string, object>{
                        {
                            "traceId", Activity.Current?.Id ?? context.TraceIdentifier
                            // Activity.Current.Id: W3C-complient | distributed-tracing (e.g. microservices) | Diagnostics
                            // HttpContext.TraceIdentifier:  // For tracking an individual local request.
                        },
                    }
                );
            }

            return Results.Ok(shoppingCart);
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
