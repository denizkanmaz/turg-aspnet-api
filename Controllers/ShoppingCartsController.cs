using Microsoft.AspNetCore.Mvc;
using Turg.App.Models;

namespace Turg.App.Controllers
{
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("2.0", Deprecated = true)]
    public class ShoppingCartsController : BaseApiController
    {
        [MapToApiVersion("1.0")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var shoppingCart = await ShoppingCart.GetById(id);

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
            var shoppingCart = await ShoppingCart.GetById(id);

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
            await ShoppingCart.AddProduct(shoppingCartItem);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        }

        [MapToApiVersion("2.0")]
        [HttpPost("{id}/items")]
        public async Task<dynamic> CreateItem([FromRoute] Guid id, [FromBody] ShoppingCartItem shoppingCartItem)
        {
            await ShoppingCart.AddProduct(shoppingCartItem, id);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        }

        [MapToApiVersion("1.0")]
        [HttpGet("Delete")]
        public async void Delete([FromQuery] string id)
        {
            await ShoppingCart.Delete(id);
        }

        [MapToApiVersion("2.0")]
        [HttpDelete("{id}")]
        public async void Remove([FromRoute] string id)
        {
            await ShoppingCart.Delete(id);
        }
    }
}
