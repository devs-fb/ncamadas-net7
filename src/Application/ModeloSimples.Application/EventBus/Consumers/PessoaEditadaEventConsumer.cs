namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaEditadaEventConsumer : IConsumer<PessoaEditadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaEditadaEvent em PessoaEditadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaEditadaEventConsumer> _logger;
    private readonly IWebhook<PessoaEditadaEvent> _webhook;

    public PessoaEditadaEventConsumer(ILogger<PessoaEditadaEventConsumer> logger, IWebhook<PessoaEditadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaEditadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}