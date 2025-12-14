using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Messages
{
    public class CustomerDeleted : IMessage
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonIgnore]
        public string MessageTypeName => nameof(CustomerDeleted);
    }
}
