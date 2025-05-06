namespace EventHub.Dtos
{
    public class BaseObjectResponse
    {
        public bool IsActive { get; set; } = true;
        public bool IsDeactivated { get; set; } = false;
        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }

    public class ReuseObjectResponse : BaseObjectResponse
    {
        public Guid Id { get; set; }
    }
}
