namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using ModeloSimples.Domain.Events;

public class PessoaJuridicaEditadaEventConsumer : IConsumer<PessoaJuridicaEditadaEvent>
{
    public Task Consume(ConsumeContext<PessoaJuridicaEditadaEvent> context)
    {
        //var pessoaJuridicaId = context.Message.pessoaJuridicaId;

        return Task.CompletedTask;
    }
}