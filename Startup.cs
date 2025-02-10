using System.Diagnostics;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Turg.App.Filters;

namespace Turg.App
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            Console.WriteLine("::Startup:: Constructor");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("::Startup:: ConfigureServices");
            services.AddScoped<LoggingFilter>();
            services.AddScoped<BenchmarkFilter>();
            services.AddScoped<CachingFilter>();

            // services.AddMvc(); // Calls AddControllersWithViews() and AddRazorPages()
            // services.AddRazorPages(); // Registers dependencies for Razor Pages
            // services.AddControllers(); // Registers dependencies for (API) Controllers
            services.AddControllers(mvcOptions =>
            {
                // MvcOptions.Filters.Add(new LoggingFilter());
                // MvcOptions.Filters.Add<LoggingFilter>(); // Type-Based global filter registration
                mvcOptions.Filters.AddService<LoggingFilter>(); // Service-Based global filter registration
            }) // Registers dependencies for Controllers and Views
            .AddXmlSerializerFormatters();

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                // options.ApiVersionReader = new HeaderApiVersionReader("X-Api-Version");
                // options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                // options.ApiVersionReader = ApiVersionReader.Combine(
                //     new HeaderApiVersionReader("X-Api-Version"),
                //     new QueryStringApiVersionReader("api-version")
                // );

                options.DefaultApiVersion = new ApiVersion(1.0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Console.WriteLine("::Startup:: Configure");

            app.UseRouting(); // Adds Endpoint Routing Middleware
            app.UseEndpoints(endpoint => // Adds Endpoint Middleware
            {
                endpoint.MapControllers();

                endpoint.MapGet("/health", async context =>
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
            });
        }
    }
}
