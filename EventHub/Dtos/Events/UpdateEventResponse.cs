using EventHub.Entities;

namespace EventHub.Dtos.Events
{
    public class UpdateEventRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IFormFile CoverImage { get; set; }
        public string Information { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EventTime { get; set; }
    }

    public class UpdateEventResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public string Information { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EventTime { get; set; }
        public ICollection<GuestSpeaker> GuestSpeakers { get; set; } = new HashSet<GuestSpeaker>();
        
    }
}
