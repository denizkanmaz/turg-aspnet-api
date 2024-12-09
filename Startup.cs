using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Turg.App.Middlewares;
using Turg.App.Services;

namespace Turg.App
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            Console.WriteLine("::Startup:: Constructor");
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("::Startup:: ConfigureServices");
            // services.Configure<HttpLoggingOptions>(options => options.LogClientBrowser = true);
            services.Configure<HttpLoggingOptions>(_configuration.GetSection("HttpLogging"));
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
