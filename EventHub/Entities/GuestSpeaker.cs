using EventHub.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventHub.Entities
{
    public class GuestSpeaker : AuditableObject
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        [JsonIgnore]
        public Guid EventId { get; set; }
        public Events Event { get; set; } 
    }
}
