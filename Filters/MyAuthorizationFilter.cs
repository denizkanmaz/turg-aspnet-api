using Microsoft.AspNetCore.Mvc.Filters;

namespace Turg.App.Filters;

internal class MyAuthorizationFilter : IAuthorizationFilter //, IAsyncAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        Console.WriteLine("MyAuthorizationFilter::OnAuthorization");
    }
}
