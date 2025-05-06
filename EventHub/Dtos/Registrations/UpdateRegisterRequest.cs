namespace EventHub.Dtos.Registrations
{
    public class UpdateRegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public IFormFile? UploadPassport { get; set; }
    }

}
