using Microsoft.AspNetCore.Mvc;

namespace Turg.App.Controllers;

[Route("api/v{v:apiVersion}/[controller]")]
[Route("[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{ }
