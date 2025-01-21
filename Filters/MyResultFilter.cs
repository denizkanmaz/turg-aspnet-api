namespace Turg.App.Filters;

using Microsoft.AspNetCore.Mvc.Filters;

internal class MyResultFilter : IResultFilter //, IAsyncResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        Console.WriteLine("MyResultFilter::OnResultExecuted");
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        Console.WriteLine("MyResultFilter::OnResultExecuting");
    }
}
