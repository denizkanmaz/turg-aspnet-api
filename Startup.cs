using Microsoft.AspNetCore.Mvc;

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
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
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
