using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Turg.App.Models;

namespace Turg.App.Endpoints;

internal class ProductEndpoints : IEndpoints
{
    public void Map(IEndpointRouteBuilder routeBuilder)
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
        }).AddEndpointFilter(async (context, next) =>
        {
            // Service Locator Pattern
            var memoryCache = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();

            var cacheKey = GenerateCacheKey(context.HttpContext.Request);

            if (memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> cachedResult))
            {
                context.HttpContext.Response.Headers.Append("X-Cache-Hit", "yes");
                return cachedResult;
            }

            // Pre-process
            var result = await next(context);
            // Post-process

            memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
            return result;
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
    }

    private string GenerateCacheKey(HttpRequest request)
    {
        var queries = request.Query.OrderBy(x => x.Key);

        var keyBuilder = new StringBuilder();
        keyBuilder.Append(request.Path);

        foreach (var query in queries)
        {
            keyBuilder.Append($"|{query.Key}-{query.Value}");
        }

        using (var md5 = MD5.Create())
        {
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(keyBuilder.ToString()));
            var hashedKey = Convert.ToBase64String(hashBytes);

            return hashedKey;
        }
    }
}
