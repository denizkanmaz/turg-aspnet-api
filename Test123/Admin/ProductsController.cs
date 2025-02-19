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

            // ViewData.Model = products;

            // var viewResult = new ViewResult()
            // {
            //     ViewName = "/Views/Admin/Products/Index.cshtml",
            //     ViewData = ViewData,
            // };

            // // await viewResult.ExecuteResultAsync(ControllerContext);

            // return viewResult;

            return View("/Views/Admin/Products/Index.cshtml", products);
        }

        [HttpGet("{productId}/manual")]
        public ActionResult Manual([FromRoute] Guid productId)
        {
            var fileName = $"manual-{productId}.pdf";

            // Sends a file that is not necessarily publicly accessible over http (e.g. wwwroot)
            // PhysicalFileResult

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/manuals", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                // return new NotFoundResult();
                // await NotifyMisssingDocument(productId)
                return NotFound();
            }

            // var result = new VirtualFileResult($"/manuals/{fileName}", "application/pdf");
            // return result;

            return File($"/manuals/{fileName}", "application/pdf");
        }
    }
}
