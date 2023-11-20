using System.Text.Json.Serialization;

namespace Turg.App
{
    public class Product
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }
        
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
