namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using ModeloSimples.Domain.Events;

public class PessoaJuridicaCriadaEventConsumer : IConsumer<PessoaJuridicaCriadaEvent>
{
    public Task Consume(ConsumeContext<PessoaJuridicaCriadaEvent> context)
    {
        //var pessoaJuridicaId = context.Message.pessoaJuridicaId;

        return Task.CompletedTask;
    }
}