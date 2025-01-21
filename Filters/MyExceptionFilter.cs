using Microsoft.AspNetCore.Mvc.Filters;

namespace Turg.App.Filters;

internal class MyExceptionFilter : IExceptionFilter //, IAsyncExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Console.WriteLine("MyExceptionFilter::OnException");
    }
}
