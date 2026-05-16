using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace EventApi.Objects
{    
    public class EventEnvelope
    {
        [Required]
        public string? EventId { get; init; }

        [Required]
        public string? Source { get; init; }

        [Required]
        public string? Timestamp { get; init; }

        [Required]
        public JsonElement? Payload { get; init; }
    }
}
