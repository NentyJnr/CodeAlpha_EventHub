using EventHub.Entities;

namespace EventHub.Dtos.Events
{
    public class EventResponse : BaseObjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? CoverImage { get; set; }
        public string Information { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? EventTime { get; set; }
        public List<GuestSpeaker> GuestSpeakers { get; set; }

    }
}
