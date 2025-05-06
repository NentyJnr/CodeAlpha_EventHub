using EventHub.Entities;

namespace EventHub.Dtos.Events
{
    public class CreateEventRequest
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile CoverImage { get; set; }
        public string Information { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EventTime { get; set; }
    }

    public class CreateEventResponse
    {
        public string Name { get; set; } = string.Empty;
        public string? CoverImage { get; set; }
        public string Information { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EventTime { get; set; }
        public List<GuestSpeaker> GuestSpeakers { get; set; } = new List<GuestSpeaker>();
        public bool IsActive { get; set; } = true;
        public bool IsDeactivated { get; set; } = false;
        public DateTime? DateCreated { get; set; } = DateTime.Now;
    }

}
