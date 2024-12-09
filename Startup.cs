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
            services.AddScoped<HttpLoggingMiddleware>();
            services.AddScoped<UserActivityService>();
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Console.WriteLine("::Startup:: Configure");

            app.UseMiddleware<HttpLoggingMiddleware>();
            app.UseRouting();
            app.UseMvc();
        }
    }
}
