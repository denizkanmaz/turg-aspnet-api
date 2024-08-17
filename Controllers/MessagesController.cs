using Microsoft.AspNetCore.Mvc;

namespace Turg.App.Controllers
{
    [Route("your-messages")]
    [Route("my-messages")]
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        [HttpGet("hello-world")]
        public String Index()
        {
            return "Hello world!";
        }
    }
}

// http://localhost:5276/your-messages/hello-world
// http://localhost:5276/my-messages/hello-world
// http://localhost:5276/messages/hello-world
