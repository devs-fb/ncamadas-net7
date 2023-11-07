namespace ModeloSimples.Domain.Base;

using ModeloSimples.Infrastructure.Shared.Interfaces;
using Newtonsoft.Json;

public abstract class AggregateRoot 
{
    [JsonIgnore]
    public IEventBus eventBus;

    [JsonIgnore]
    private readonly List<IEvent> _domainEvents = new();

    [JsonIgnore]
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents;

    protected void RegistrarEvento(IEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void LimparEventos() => _domainEvents.Clear();
}
