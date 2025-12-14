using System.Text.Json.Serialization;

namespace Speakers.Models
{
    public class Speaker
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("bio")]
        public string Bio { get; set; } = default!;

        [JsonPropertyName("webSite")]
        public string WebSite { get; set; } = default!;
    }

}
