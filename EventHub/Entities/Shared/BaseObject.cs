namespace EventHub.Entities.Shared
{
    public abstract class BaseObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
