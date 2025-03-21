using Microsoft.AspNetCore.Mvc;
using Turg.App.Models;
using Turg.App.Persistence;

namespace Turg.App.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0", Deprecated = true)]
    public class ShoppingCartsController : BaseApiController
    {
        private readonly ShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartsController(IServiceProvider services)
        {
            _shoppingCartRepository = services.GetRequiredService<ShoppingCartRepository>();
        }

        [MapToApiVersion("1.0")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var shoppingCart = await _shoppingCartRepository.GetById(id);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }

        [MapToApiVersion("2.0")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var shoppingCart = await _shoppingCartRepository.GetById(id);

            if (shoppingCart == null)
            {
                return NotFound();
            }

            return Ok(shoppingCart);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("AddProduct")]
        public async Task<dynamic> AddProduct([FromBody] ShoppingCartItem shoppingCartItem)
        {
            await _shoppingCartRepository.AddProduct(shoppingCartItem);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{id}/items")]
        public async Task<dynamic> CreateItem([FromRoute] Guid id, [FromBody] ShoppingCartItem shoppingCartItem)
        {
            await _shoppingCartRepository.AddProduct(shoppingCartItem, id);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        }

        [MapToApiVersion("1.0")]
        [HttpGet("Delete")]
        public async void Delete([FromQuery] string id)
        {
            await _shoppingCartRepository.Delete(id);
        }

        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public async void Remove([FromRoute] string id)
        {
            await _shoppingCartRepository.Delete(id);
        }
    }
}
