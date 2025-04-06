using System.Diagnostics;
using System.Reflection;
using Turg.App.Endpoints;
using Turg.App.Infrastructure;
using Turg.App.Middleware;
using Turg.App.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureServices
builder.Services.AddScoped<CachingFilter>();
builder.Services.AddScoped<LoggingMiddleware>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers()
.AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(options =>
{
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.DefaultApiVersion = new ApiVersion(1.0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddMvc();

builder.Services.Configure<SqlConnectionOptions>(builder.Configuration.GetSection("SqlConnection"));
builder.Services.AddSingleton<ISqlCommandExecutor, NpgsqlCommandExecutor>();
builder.Services.AddSingleton<ISqlConnectionFactory, NpgsqlConnectionFactory>();

var useMock = builder.Configuration.GetValue<bool>("UseMockRepository");

if (useMock)
{
    builder.Services.AddSingleton<IProductRepository, ProductRepositoryMock>();
    builder.Services.AddSingleton<IShoppingCartRepository, ShoppingCartRepositoryMock>();
}
else
{
    builder.Services.AddSingleton<IProductRepository, ProductRepository>();
    builder.Services.AddSingleton<IShoppingCartRepository, ShoppingCartRepository>();
}

#endregion

var app = builder.Build();

// var productRepository = app.Services.GetRequiredService<ProductRepository>();

#region Configure
app.UseMiddleware<LoggingMiddleware>();

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

var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(3, 0))
    // .HasApiVersion(new ApiVersion(4, 0))
    .Build();

app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(versionSet)
    .MapEndpoints(Assembly.GetExecutingAssembly());

app.MapControllers();
#endregion

app.Run();
