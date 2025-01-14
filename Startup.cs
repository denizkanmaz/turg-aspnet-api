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
            app.Use(async (context, next) =>
            {
                var isSingleTransmissionEnabled = context.Request.Headers["X-Request-Response-Mode"].ToString().Equals("Single-Transmission", StringComparison.OrdinalIgnoreCase);

                if (!isSingleTransmissionEnabled)
                {
                    await next();
                    return;
                }

                var originalBody = context.Response.Body;

                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                // Pre-process
                await next();
                // Post-process

                memoryStream.Seek(0, SeekOrigin.Begin);
                context.Response.ContentLength = memoryStream.Length;
                await memoryStream.CopyToAsync(originalBody);
                context.Response.Body = originalBody;
            });

            app.Use(async (context, next) =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await context.Response.WriteAsync($"Chunk {i}\n");
                    await Task.Delay(1000);
                }

                await next();
            });
        }
    }
}
