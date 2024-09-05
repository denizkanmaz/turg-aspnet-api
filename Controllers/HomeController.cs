using Microsoft.AspNetCore.Mvc;

namespace Turg.App.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
