namespace EventHub.Dtos.Tags
{
    public class TagResponse
    {
        public string uniqueId { get; set; }
        public string SignatureUrl { get; set; }
        public string LogoUrl { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string EventName { get; set; }
        public string? Reference { get; set; }
    }
}
