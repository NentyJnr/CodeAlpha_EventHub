namespace EventHub.Dtos.EventUploads
{
    public class EventUploadRequest
    {
        public IFormFile? UploadLogo { get; set; }
        public IFormFile? UploadSignature { get; set; }
        public Guid EventId { get; set; }
    }

    public class EventUploadResponse
    {
        public string? LogoUrl { get; set; }
        public string? SignatureUrl { get; set; }
        public Guid EventId { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
