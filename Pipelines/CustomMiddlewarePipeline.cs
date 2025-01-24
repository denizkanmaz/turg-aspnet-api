namespace Turg.App.Pipelines;

/// <summary>
/// This class is responsible for configuring the custom middleware pipeline for the MVC Request Life Cycle.
/// </summary>
internal class CustomMiddlewarePipeline
{
    public void Configure(IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            Console.WriteLine("Custom middleware pipeline in MVC - Middleware #1 - Pre-process");
            await next();
            Console.WriteLine("Custom middleware pipeline in MVC - Middleware #1 - Post-process");
        });

        app.Use(async (context, next) =>
        {
            Console.WriteLine("Custom middleware pipeline in MVC - Middleware #2 - Pre-process");
            await next();
            Console.WriteLine("Custom middleware pipeline in MVC - Middleware #2 - Post-process");
        });

        // app.UseMiddleware<MyConventionBasedMiddleware>();
        // app.UseMiddleware<MyFactoryBasedMiddleware>();
        // app.UseMyMiddleware();
    }
}
