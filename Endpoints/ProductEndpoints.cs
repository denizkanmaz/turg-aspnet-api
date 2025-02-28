using Microsoft.AspNetCore.Http.HttpResults;
using Turg.App.Models;

namespace Turg.App.Endpoints;

internal class ProductEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        var productsGroup = routeBuilder
            .MapGroup("/products")
            .MapToApiVersion(3, 0);

        productsGroup.MapGet("/", async (string category) =>
        {
            if (!String.IsNullOrWhiteSpace(category))
            {
                var productsByCategory = await Product.GetByCategory(category);
                return productsByCategory;
            }

            var products = await Product.GetAll();
            return products;
        }).AddEndpointFilter<CachingEndpointFilter>();
        // .MapToApiVersion(3, 0);

        // productsGroup.MapGet("/", async (string category) =>
        // {
        //     return "This is going to be version 4";
        // }).AddEndpointFilter<CachingEndpointFilter>()
        // .MapToApiVersion(4, 0);

        productsGroup.MapPost("/", async ValueTask<Results<ValidationProblem, Ok<object>>> (HttpContext context, Product product) =>
        {
            var id = await Product.Add(product);
            return TypedResults.Ok<object>(new { Result = "OK", Message = "Product added", Id = id });
        }).AddEndpointFilter<ValidationEndpointFilter<Product>>();

        productsGroup.MapPut("/{id}", async (Guid id, Product product) =>
        {
            await Product.Update(product, id);
            return new { Result = "OK", Message = "Product updated" };
        }).AddEndpointFilter<ValidationEndpointFilter<Product>>();
    }
}
