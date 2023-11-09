namespace ModeloSimples.Application.EventBus.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Events;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaJuridicaCriadaEventConsumer : IConsumer<PessoaJuridicaCriadaEvent>
{
    private const string EventoRecebidoFormat = "Evento Recebido PessoaJuridicaCriadaEvent em PessoaJuridicaCriadaEventConsumer com o ID {0}";

    private readonly ILogger<PessoaJuridicaCriadaEventConsumer> _logger;
    private readonly IWebhook<PessoaJuridicaCriadaEvent> _webhook;

    public PessoaJuridicaCriadaEventConsumer(ILogger<PessoaJuridicaCriadaEventConsumer> logger, IWebhook<PessoaJuridicaCriadaEvent> webhook)
    {
        _logger = logger;
        _webhook = webhook;
    }

    public async Task Consume(ConsumeContext<PessoaJuridicaCriadaEvent> context)
    {
        var pessoaId = context.Message.PessoaJuridicaId;

        await _webhook.EnviarEventoAsync(context.Message);

        _logger.LogInformation(string.Format(EventoRecebidoFormat, pessoaId));
    }
}