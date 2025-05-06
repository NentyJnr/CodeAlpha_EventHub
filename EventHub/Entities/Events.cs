using EventHub.Entities.Shared;

namespace EventHub.Entities
{
    public class Events : AuditableObject
    {
        public string Name { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public string Information { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EventTime { get; set; }
        public List<GuestSpeaker> GuestSpeakers { get; set; } = new List<GuestSpeaker>();
    }
}
