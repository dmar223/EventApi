using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventApi.Objects
{    
    public class EventEnvelope
    {
        [Required]
        [JsonPropertyName("event_id")]
        public string? EventId { get; init; }

        [Required]
        [JsonPropertyName("source")]
        public string? Source { get; init; }

        [Required]
        [JsonPropertyName("timestamp")]
        public string? Timestamp { get; init; }

        [Required]
        [JsonPropertyName("payload")]
        public JsonElement? Payload { get; init; }
    }
}
