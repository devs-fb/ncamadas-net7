namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaRemovidaEventConsumer : IConsumer<PessoaRemovidaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaRemovidaEvent em PessoaRemovidaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaRemovidaEventConsumer> _logger;
    private readonly IWebhook<PessoaRemovidaEvent> _webhook;

    public PessoaRemovidaEventConsumer(ILogger<PessoaRemovidaEventConsumer> logger, IWebhook<PessoaRemovidaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaRemovidaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}