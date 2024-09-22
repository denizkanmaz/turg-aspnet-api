using Microsoft.AspNetCore.Mvc;
using Turg.App.Models;

namespace Turg.App.Controllers.Admin
{
    [Route("admin/[controller]")]
    public class ProductsController : Controller
    {
        public async Task<ViewResult> Index()
        {
            var products = await Product.GetAll();
            return View("/Views/Admin/Products/Index.cshtml", products);
        }
    }
}
