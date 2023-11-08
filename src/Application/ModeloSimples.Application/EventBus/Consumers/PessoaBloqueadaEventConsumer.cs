namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaBloqueadaEventConsumer : IConsumer<PessoaBloqueadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaBloqueadaEvent em PessoaBloqueadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaBloqueadaEventConsumer> _logger;
    private readonly IWebhook<PessoaBloqueadaEvent> _webhook;

    public PessoaBloqueadaEventConsumer(ILogger<PessoaBloqueadaEventConsumer> logger, IWebhook<PessoaBloqueadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaBloqueadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}