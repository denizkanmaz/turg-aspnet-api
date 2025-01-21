using Microsoft.AspNetCore.Mvc.Filters;

namespace Turg.App.Filters;

internal class MyActionFilter : IActionFilter //, IAsyncActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        Console.WriteLine("MyActionFilter::OnActionExecuted");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        Console.WriteLine("MyActionFilter::OnActionExecuting");
    }
}
