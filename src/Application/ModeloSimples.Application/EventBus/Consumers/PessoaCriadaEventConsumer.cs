namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaCriadaEventConsumer : IConsumer<PessoaCriadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaCriadaEvent em PessoaCriadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaCriadaEventConsumer> _logger;
    private readonly IWebhook<PessoaCriadaEvent> _webhook;

    public PessoaCriadaEventConsumer(ILogger<PessoaCriadaEventConsumer> logger, IWebhook<PessoaCriadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaCriadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}