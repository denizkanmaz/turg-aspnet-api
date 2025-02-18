using Turg.App.Models;

namespace Turg.App.Endpoint;

internal static class ProductEndpoints
{
    internal static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var productsGroup = routeBuilder.MapGroup("/products");

        productsGroup.MapGet("/", async (string category) =>
        {
            if (!String.IsNullOrWhiteSpace(category))
            {
                var productsByCategory = await Product.GetByCategory(category);
                return productsByCategory;
            }

            var products = await Product.GetAll();
            return products;
        });

        productsGroup.MapPost("/", async (Product product) =>
        {
            var id = await Product.Add(product);
            return new { Result = "OK", Message = "Product added", Id = id };
        });

        productsGroup.MapPut("/{id}", async (Guid id, Product product) =>
        {
            await Product.Update(product, id);
            return new { Result = "OK", Message = "Product updated" };
        });

        return routeBuilder;
    }
}
