namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaFisicaEditadaEventConsumer : IConsumer<PessoaFisicaEditadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaFisicaEditadaEvent em PessoaFisicaEditadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaFisicaEditadaEventConsumer> _logger;
    private readonly IWebhook<PessoaFisicaEditadaEvent> _webhook;

    public PessoaFisicaEditadaEventConsumer(ILogger<PessoaFisicaEditadaEventConsumer> logger, IWebhook<PessoaFisicaEditadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaFisicaEditadaEvent> context)
    {
        var pessoaId = context.Message.PessoaFisicaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}