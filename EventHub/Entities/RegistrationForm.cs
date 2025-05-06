using EventHub.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Entities
{
    public class RegistrationForm : AuditableObject
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? UploadPassport { get; set; }
        public Guid EventId { get; set; }
        public Events Events { get; set; }
    }
}
