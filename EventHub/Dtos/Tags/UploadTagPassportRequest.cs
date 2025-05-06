namespace EventHub.Dtos.Tags
{
    public class UploadTagPassportRequest
    {
        public IFormFile file { get; set; }
        public string reference { get; set; }
        public Guid EventId { get; set; }
    }
}

