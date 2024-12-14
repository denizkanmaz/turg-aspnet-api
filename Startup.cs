using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Turg.App.Middlewares;
using Turg.App.Services;

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
            services.AddScoped<UserActivityService>();
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Console.WriteLine("::Startup:: Configure");

            // Trust "X-Forwarded-For" header
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor
            });

            app.UseMiddleware<HttpLoggingMiddleware>();
            app.UseMiddleware<BlacklistingMiddleware>();
            app.UseRouting();
            app.UseMvc();
        }
    }
}
