using EventHub.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventHub.Dtos.Registrations
{
    public class RegisterResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid EventId { get; set; }
        public string Reference {  get; set; }
        public string PhoneNumber { get; set; } = string.Empty; 
        public string PassportUrl { get; set; } = "default-passport.png"; 
    }
}
