namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaFisicaCriadaEventConsumer : IConsumer<PessoaFisicaCriadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaFisicaCriadaEvent em PessoaFisicaCriadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaFisicaCriadaEventConsumer> _logger;
    private readonly IWebhook<PessoaFisicaCriadaEvent> _webhook;

    public PessoaFisicaCriadaEventConsumer(ILogger<PessoaFisicaCriadaEventConsumer> logger, IWebhook<PessoaFisicaCriadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaFisicaCriadaEvent> context)
    {
        var pessoaId = context.Message.PessoaFisicaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}