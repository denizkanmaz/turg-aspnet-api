using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Turg.App.Models;
using Turg.App.Persistence;

namespace Turg.App.Endpoints;

internal class ProductEndpoints() : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
    {
        var productsGroup = routeBuilder
            .MapGroup("/products")
            .MapToApiVersion(3, 0);

        productsGroup.MapGet("/", Get).WithCaching();
        productsGroup.MapPost("/", Post).WithValidation<Product>();
        productsGroup.MapPut("/{id}", Put).WithValidation<Product>();
    }

    private static async Task<Ok<IEnumerable<Product>>> Get([FromServices] IProductRepository productRepository, string category)
    {
        var products = await productRepository.Get(category);

        return TypedResults.Ok(products);
    }

    private static async Task<Results<ValidationProblem, Ok<object>>> Post([FromServices] IProductRepository productRepository, HttpContext context, Product product)
    {
        var id = await productRepository.Insert(product);
        return TypedResults.Ok<object>(new { Result = "OK", Message = "Product added", Id = id });
    }

    private static async Task<Ok<object>> Put([FromServices] IProductRepository productRepository, Guid id, Product product)
    {
        await productRepository.Update(product, id);
        return TypedResults.Ok<object>(new { Result = "OK", Message = "Product updated" });
    }
}
