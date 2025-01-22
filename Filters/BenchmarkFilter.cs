using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Turg.App.Filters;

internal class BenchmarkFilter : IActionFilter
{
    private readonly ILogger<BenchmarkFilter> _logger;
    private readonly Stopwatch _stopwatch;

    public BenchmarkFilter(ILogger<BenchmarkFilter> logger)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _stopwatch.Start();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _stopwatch.Stop();

        var controllerName = context.RouteData.Values["controller"];
        var actionName = context.RouteData.Values["action"];

        _logger.LogInformation($"{controllerName} - {actionName} executed in {_stopwatch.ElapsedMilliseconds}ms");
    }
}
