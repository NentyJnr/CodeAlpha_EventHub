namespace EventHub.Dtos.EventUploads
{
    public class UploadResponse
    {
        public Guid Id { get; set; }
        public string? LogoUrl { get; set; }
        public string? SignatureUrl { get; set; }
        public Guid EventId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeactivated { get; set; } = false;
    }
}
