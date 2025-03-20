using System.Text.Json.Serialization;

namespace Turg.App.Models
{
    public class ShoppingCart
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }

        [JsonPropertyName("totalPrice")]
        public double TotalPrice { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
