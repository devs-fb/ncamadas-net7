namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaJuridicaEditadaEventConsumer : IConsumer<PessoaJuridicaEditadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaJuridicaEditadaEvent em PessoaBloqueadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaJuridicaEditadaEventConsumer> _logger;
    private readonly IWebhook<PessoaJuridicaEditadaEvent> _webhook;

    public PessoaJuridicaEditadaEventConsumer(ILogger<PessoaJuridicaEditadaEventConsumer> logger, IWebhook<PessoaJuridicaEditadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaJuridicaEditadaEvent> context)
    {
        var pessoaId = context.Message.PessoaJuridicaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}