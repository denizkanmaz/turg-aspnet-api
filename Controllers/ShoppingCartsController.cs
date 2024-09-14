using Microsoft.AspNetCore.Mvc;
using Turg.App.Models;

namespace Turg.App.Controllers
{
    [Route("[controller]")]
    public class ShoppingCartsController : Controller
    {
        // Returns a shopping cart by id.
        // GET: /shoppingcarts/GetById?id=ae8fbf0c-4acf-47c6-a1ca-f429f6b17e2d
        [HttpGet("GetById")]
        public async Task<ShoppingCart> GetById([FromQuery] string id)
        {
            var shoppingCart = await ShoppingCart.GetById(id);
            return shoppingCart;
        }

        // Adds a shopping cart item to a shopping cart.
        // GET: /shoppingcarts/AddProduct
        // {
        //     "shoppingCartId": "ae8fbf0c-4acf-47c6-a1ca-f429f6b17e2d",
        //     "productId": "8b2aedf0-c6a8-4a09-a36a-077055a37133",
        //     "quantity": 1
        // }
        [HttpGet("AddProduct")]
        public async Task<dynamic> AddProduct([FromBody] ShoppingCartItem shoppingCartItem)
        {
            await ShoppingCart.AddProduct(shoppingCartItem);
            return new { Result = "OK", Message = "Product added to shopping cart" };
        }

        // Deletes a shopping cart and its items.
        // GET: /shoppingcarts/Delete?id=ae8fbf0c-4acf-47c6-a1ca-f429f6b17e2d
        [HttpGet("Delete")]
        public async void Delete([FromQuery] string id)
        {
            await ShoppingCart.Delete(id);
        }
    }
}
