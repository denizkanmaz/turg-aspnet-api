using Microsoft.AspNetCore.Mvc.Filters;

namespace Turg.App.Filters;

internal class MyResourceFilter : IResourceFilter //, IAsyncResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        Console.WriteLine("MyResourceFilter::OnResourceExecuted");
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        Console.WriteLine("MyResourceFilter::OnResourceExecuting");
    }
}
