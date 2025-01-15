using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

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
            services.AddMvc(MvcOptions =>
            {
                MvcOptions.EnableEndpointRouting = false;
                MvcOptions.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            Console.WriteLine("::Startup:: Configure");
            app.UseRouting();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
