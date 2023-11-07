namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using ModeloSimples.Domain.Events;

public class PessoaEditadaEventConsumer : IConsumer<PessoaEditadaEvent>
{
    public Task Consume(ConsumeContext<PessoaEditadaEvent> context)
    {
        //var pessoaId = context.Message.PessoaId;

        return Task.CompletedTask;
    }
}