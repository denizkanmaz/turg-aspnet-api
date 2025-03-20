using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Npgsql;

namespace Turg.App.Models
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
        [Length(3, 3, ErrorMessage = "Currency must follow ISO-*** format.")]
        public string Currency { get; set; }
    }
}
