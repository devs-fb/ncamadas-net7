namespace ModeloSimples.Domain.Base;

public abstract class DomainEvent
{
    public DateTime Timestamp { get; private set; }
    public Guid EventId { get; private set; }

    protected DomainEvent()
    {
        Timestamp = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }
}
