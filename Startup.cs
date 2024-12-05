using System.Diagnostics;
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

            // anonymous inline delegate (middleware)
            app.Use(async (context, next) =>
           {
               var stopwatch = Stopwatch.StartNew();

               // Resolve from DI container
               var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();

               var request = context.Request;

               logger.LogInformation("[{Timestamp}] Request: {Method} {Path} from {ClientIP}",
               DateTime.UtcNow,
               request.Method,
               request.Path,
               context.Connection.RemoteIpAddress);

               // pre-processing
               await next();
               // post-processing

               stopwatch.Stop();

               logger.LogInformation("[{Timestamp}] Response: {StatusCode} in {ElapsedMilliseconds} ms",
               DateTime.UtcNow,
               context.Response.StatusCode,
               stopwatch.ElapsedMilliseconds);
           });

            app.UseRouting();
            app.UseMvc();
        }
    }
}
