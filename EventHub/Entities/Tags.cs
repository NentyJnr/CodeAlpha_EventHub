using EventHub.Entities.Shared;

namespace EventHub.Entities
{
    public class Tags : AuditableObject
    {
        public string LogoUrl { get; set; } = string.Empty;
        public string SignatureUrl { get; set; } = string.Empty;
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public Guid EventId { get; set; }
        public Events Event { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string PassportUrl { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public Guid RegistrationId { get; set; }
    }
}
