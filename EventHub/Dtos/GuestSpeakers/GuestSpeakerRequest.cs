using System.ComponentModel.DataAnnotations;

namespace EventHub.Dtos.GuestSpeakers
{
    public class GuestSpeakerRequest
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public string Biography { get; set; } = string.Empty;
        [Required] public string Topic { get; set; } = string.Empty;
        [Required] public Guid EventId { get; set; }
        [Required] public IFormFile Image { get; set; }
    }

    public class GuestSpeakerResponse : ReuseObjectResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public Guid EventId { get; set; }
        public string Image { get; set; }
    }

}
