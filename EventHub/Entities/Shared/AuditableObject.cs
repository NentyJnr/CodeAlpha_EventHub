namespace EventHub.Entities.Shared
{
    public abstract class AuditableObject : BaseObject
    {
        public DateTime? DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateModified { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }

        public bool IsDeactivated { get; set; }
    }
}
