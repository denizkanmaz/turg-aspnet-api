using Microsoft.AspNetCore.Mvc;

namespace Turg.App
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        { }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(MvcOptions => MvcOptions.EnableEndpointRouting = false);
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseRouting();
            app.UseMvc();
        }
    }
}
