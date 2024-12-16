using Microsoft.AspNetCore.Mvc;
using Turg.App.Middlewares.ClientMetadataParsing;
using Turg.App.Middlewares.HttpLogging;

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

            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
            services.AddCustomHttpLogging(_configuration);
            services.AddClientMetadataParsing();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Console.WriteLine("::Startup:: Configure");

            app.UseClientMetadataParsing();
            app.UseCustomHttpLogging();
            app.UseRouting();
            app.UseMvc();
        }
    }
}
