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
            // app.UseRouting();
            // app.UseMvc();

            // Sales office
            app.Use(async (context, next) =>
            {
                Console.WriteLine("Sales office");

                bool isDeal = context.Request.Headers["x-deal"] == "yes";

                if (!isDeal)
                {
                    Console.WriteLine("Sales office - No deal");
                    context.Response.StatusCode = 401;
                    return;
                }

                await next();
            });
            
            // Parts tracking station
            app.Use(async (context, next) =>
            {
                Console.WriteLine("Parts tracking system - Expectations logged (Pre-process)");

                await next();

                Console.WriteLine("Parts tracking system - Serial numbers logged (Post-process)");
            });

            // Chassis-frame station
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Chasis");
                Console.WriteLine("Chassis-Frame station - Frame added");

                await next();
            });

            // Engine assembly station
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("+Engine");
                Console.WriteLine("Engine assembly station - Engine assembled");

                await next();
            });
        }
    }
}
