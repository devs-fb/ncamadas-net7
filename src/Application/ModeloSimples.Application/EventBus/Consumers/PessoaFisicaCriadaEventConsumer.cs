namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using ModeloSimples.Domain.Events;

public class PessoaFisicaCriadaEventConsumer : IConsumer<PessoaFisicaCriadaEvent>
{
    public Task Consume(ConsumeContext<PessoaFisicaCriadaEvent> context)
    {
        //var pessoaFisicaId = context.Message.pessoaFisicaId;

        return Task.CompletedTask;
    }
}