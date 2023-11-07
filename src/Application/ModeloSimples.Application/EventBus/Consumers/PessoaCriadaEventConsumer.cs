namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;

public class PessoaCriadaEventConsumer : IConsumer<PessoaCriadaEvent>
{
    private readonly ILogger<PessoaCriadaEventConsumer> _logger;

    public PessoaCriadaEventConsumer(ILogger<PessoaCriadaEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PessoaCriadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        _logger.LogInformation($"Evento Recebido PessoaCriadaEvent em PessoaCriadaEventConsumer com o ID {pessoaId}");

        return Task.CompletedTask;
    }
}