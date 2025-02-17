using System.Diagnostics;
using Turg.App.Models;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureServices
builder.Services.AddScoped<LoggingFilter>()
                    .AddScoped<CachingFilter>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers(mvcOptions =>
{
    mvcOptions.Filters.AddService<LoggingFilter>();
})
.AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.DefaultApiVersion = new ApiVersion(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
}).AddMvc();
#endregion

var app = builder.Build();

#region Configure
app.MapGet("/health", async context =>
     {
         var startTime = Process.GetCurrentProcess().StartTime.ToUniversalTime();
         var uptime = DateTime.UtcNow - startTime;

         var uptimeString = $"{uptime.Hours}H {uptime.Minutes}M, {uptime.Seconds}S";

         var healthStatus = new
         {
             Status = "Running",
             Uptime = uptimeString,
             Environment.MachineName,
             OS = Environment.OSVersion.Platform.ToString(),
             Environment.ProcessId,
             Environment.ProcessorCount,
         };

         await context.Response.WriteAsJsonAsync(healthStatus);
     });


var apiV3Group = app.MapGroup("/api/v3.0-dev");

var productsGroup = apiV3Group.MapGroup("/products");

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

var shoppingCartGroup = apiV3Group.MapGroup("/shoppingcarts");
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

app.MapControllers();
#endregion

app.Run();
