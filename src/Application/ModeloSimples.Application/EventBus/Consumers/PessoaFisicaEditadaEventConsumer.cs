namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using ModeloSimples.Domain.Events;

public class PessoaFisicaEditadaEventConsumer : IConsumer<PessoaFisicaEditadaEvent>
{
    public Task Consume(ConsumeContext<PessoaFisicaEditadaEvent> context)
    {
        //var pessoaFisicaId = context.Message.pessoaFisicaId;

        return Task.CompletedTask;
    }
}