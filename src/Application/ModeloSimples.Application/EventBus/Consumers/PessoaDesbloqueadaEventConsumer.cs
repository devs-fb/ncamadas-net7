namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;

public class PessoaDesbloqueadaEventConsumer : IConsumer<PessoaDesbloqueadaEvent>
{
    private readonly ILogger<PessoaDesbloqueadaEventConsumer> _logger;

    public PessoaDesbloqueadaEventConsumer(ILogger<PessoaDesbloqueadaEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PessoaDesbloqueadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        _logger.LogInformation($"Evento Recebido PessoaDesbloqueadaEvent em PessoaDesbloqueadaEventConsumer com o ID {pessoaId}");

        return Task.CompletedTask;
    }
}