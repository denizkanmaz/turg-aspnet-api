using System.Diagnostics;
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

            services.AddMvc(MvcOptions =>
            {
                MvcOptions.EnableEndpointRouting = false;
                MvcOptions.OutputFormatters.Add(new XmlSerializerOutputFormatter());

                // MvcOptions.Filters.Add(new LoggingFilter());
                // MvcOptions.Filters.Add<LoggingFilter>(); // Type-Based global filter registration
                MvcOptions.Filters.AddService<LoggingFilter>(); // Service-Based global filter registration
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Console.WriteLine("::Startup:: Configure");
            app.UseRouting();
            app.UseStaticFiles();
            app.UseMvc();

            app.UseEndpoints(endpoint =>
            {
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
