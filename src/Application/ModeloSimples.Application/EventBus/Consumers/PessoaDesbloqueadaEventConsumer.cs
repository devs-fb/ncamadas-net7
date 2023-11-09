namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaDesbloqueadaEventConsumer : IConsumer<PessoaDesbloqueadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaDesbloqueadaEvent em PessoaDesbloqueadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaDesbloqueadaEventConsumer> _logger;
    private readonly IWebhook<PessoaDesbloqueadaEvent> _webhook;

    public PessoaDesbloqueadaEventConsumer(ILogger<PessoaDesbloqueadaEventConsumer> logger, IWebhook<PessoaDesbloqueadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaDesbloqueadaEvent> context)
    {
        var pessoaId = context.Message.PessoaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}