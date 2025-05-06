using EventHub.Entities.Shared;

namespace EventHub.Entities
{
    public class EventUploads : AuditableObject
    {
        public string SignatureUrl { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public Guid EventId { get; set; }
    }
}
