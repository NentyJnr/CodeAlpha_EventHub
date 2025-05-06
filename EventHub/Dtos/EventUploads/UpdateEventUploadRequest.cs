namespace EventHub.Dtos.EventUploads
{
    public class UpdateEventUploadRequest
    {
        public Guid Id { get; set; }
        public IFormFile? UploadLogo { get; set; }
        public IFormFile? UploadSignature { get; set; }
    }

    public class UpdateEventUploadResponse
    {
        public string? UploadLogo { get; set; }
        public string? UploadSignature { get; set; }
        public Guid EventId { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
