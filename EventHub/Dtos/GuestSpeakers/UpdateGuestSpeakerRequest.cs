namespace EventHub.Dtos.GuestSpeakers
{
    public class UpdateGuestSpeakerRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
    }

    public class UpdateGuestSpeakerResponse
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public string Image { get; set; }
        public DateTime DateModified { get; set; }
    }
}
