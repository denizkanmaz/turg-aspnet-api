using System;
using System.Diagnostics;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Turg.App.Filters;

namespace Turg.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<LoggingFilter>()
            .AddScoped<CachingFilter>();

            services.AddMemoryCache();
            
            services.AddControllers(mvcOptions =>
            {
                mvcOptions.Filters.AddService<LoggingFilter>();
            })
            .AddXmlSerializerFormatters();

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.DefaultApiVersion = new ApiVersion(1.0);
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseRouting();
            app.UseEndpoints(endpoint =>
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
