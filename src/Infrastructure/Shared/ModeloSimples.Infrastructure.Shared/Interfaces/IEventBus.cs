namespace ModeloSimples.Infrastructure.Shared.Interfaces;

public interface IEventBus
{
    Task Publish<T>(T message) where T : class;
    Task PublishRange(IEnumerable<IEvent> messages);
}
