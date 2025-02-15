using System;
using System.Text.Json.Serialization;

namespace Turg.App.Models
{
    public class ShoppingCartItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("shoppingCartId")]
        public Guid? ShoppingCartId { get; set; }
    }
}
