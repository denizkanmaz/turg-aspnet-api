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

            ViewData.Model = products;

            var viewResult = new ViewResult()
            {
                ViewName = "/Views/Admin/Products/Index.cshtml",
                ViewData = ViewData,
            };

            // await viewResult.ExecuteResultAsync(ControllerContext);

            return viewResult;

            // return View("/Views/Admin/Products/Index.cshtml", products);
        }
    }
}
