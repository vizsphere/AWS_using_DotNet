using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Messages
{
    public class CustomerCreated : IMessage
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonIgnore]
        public string MessageTypeName => nameof(CustomerCreated);
    }
}
