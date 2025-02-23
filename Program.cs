using System.Diagnostics;
using System.Reflection;
using Turg.App.Endpoints;
using Turg.App.Middleware;

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
}).AddMvc();
#endregion

var app = builder.Build();

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

app.MapGroup("/api/v3.0-dev")
    .MapEndpoints(Assembly.GetExecutingAssembly());

app.MapControllers();
#endregion

app.Run();
