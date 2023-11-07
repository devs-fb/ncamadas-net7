namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;

public class PessoaBloqueadaEventConsumer : IConsumer<PessoaBloqueadaEvent>
{
    private readonly ILogger<PessoaBloqueadaEventConsumer> _logger;

    public PessoaBloqueadaEventConsumer(ILogger<PessoaBloqueadaEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PessoaBloqueadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        _logger.LogInformation($"Evento Recebido PessoaBloqueadaEvent em PessoaBloqueadaEventConsumer com o ID {pessoaId}");

        return Task.CompletedTask;
    }
}