namespace ModeloSimples.Application.EventBus;

using MassTransit;

public class PublishErrorHandler : IConsumer<ReceiveFault>
{
    public async Task Consume(ConsumeContext<ReceiveFault> context)
    {
        // Obter a exceção que causou a falha
        var exception = context.Message.Exceptions.First(); 

        await context.Publish(exception);
    }
}
