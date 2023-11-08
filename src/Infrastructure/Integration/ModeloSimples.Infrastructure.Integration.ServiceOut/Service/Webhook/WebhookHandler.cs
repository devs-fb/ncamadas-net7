namespace ModeloSimples.Infrastructure.Integration.ServiceOut.Service.Webhook;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Interfaces;
using Newtonsoft.Json;
using System.Text;

public class WebhookHandler<T> : IWebhook<T>
{
    private readonly IHttpClientService _httpClientService;
    private readonly IDictionary<string, List<string>> _subscribers;
    private readonly IDistributedCache _cache;
    private readonly ILogger<WebhookHandler<T>> _logger;
    private readonly string _cacheKey = $"{typeof(T).Name}_Subscribers";

    private const string InscritoAdicionadoFormat = "Inscrito adicionado para o evento {0}: {1}";
    private const string InscritoRemovidoFormat = "Inscrito removido para o evento {0}: {1}";
    private const string TentativaRemoverInscritoNaoExistenteFormat = "Tentativa de remover inscrito para o evento {0} que não existe: {1}";
    private const string EventoEnviadoFormat = "Evento {0} enviado para: {1}";
    private const string ErroAoEnviarEventoFormat = "Erro ao enviar evento {0} para {1}: {2}";
    private const string NenhumInscritoEncontradoFormat = "Nenhum inscrito encontrado para o evento {0}";
    private const string ApplicationJson = "application/json";

    public WebhookHandler(IHttpClientService httpClientService, IDistributedCache cache, ILogger<WebhookHandler<T>> logger)
    {
        _httpClientService = httpClientService;
        _subscribers = new Dictionary<string, List<string>>();
        _cache = cache;
        _logger = logger;

        var cachedSubscribers = _cache.GetString(_cacheKey);
        if (!string.IsNullOrEmpty(cachedSubscribers))
        {
            _subscribers[typeof(T).Name] = JsonConvert.DeserializeObject<List<string>>(cachedSubscribers) as List<string> ?? new();
        }
    }

    public void AdicionarInscrito(string webhookUrl)
    {
        if (_subscribers.ContainsKey(typeof(T).Name))
        {
            _subscribers[typeof(T).Name].Add(webhookUrl);
        }
        else
        {
            _subscribers[typeof(T).Name] = new List<string> { webhookUrl };
        }

        AtualizarCache();

        _logger.LogInformation(string.Format(InscritoAdicionadoFormat, typeof(T).Name, webhookUrl));
    }

    public void RemoverInscrito(string webhookUrl)
    {
        if (_subscribers.ContainsKey(typeof(T).Name))
        {
            _subscribers[typeof(T).Name].Remove(webhookUrl);
            _logger.LogInformation(string.Format(InscritoRemovidoFormat, typeof(T).FullName, webhookUrl));

            AtualizarCache();
        }
        else
        {
            _logger.LogWarning(string.Format(TentativaRemoverInscritoNaoExistenteFormat, typeof(T).Name, webhookUrl));
        }
    }

    public async Task EnviarEventoAsync(T evento)
    {
        if (_subscribers.TryGetValue(typeof(T).Name, out var subscriberUrls))
        {
            var eventoComNome = new { EventType = typeof(T).Name, Data = evento };
            var jsonData = JsonConvert.SerializeObject(eventoComNome);
            var content = new StringContent(jsonData, Encoding.UTF8, ApplicationJson);

            foreach (var subscriberUrl in subscriberUrls)
            {
                try
                {
                    await _httpClientService.PostAsync(subscriberUrl, content);
                    _logger.LogInformation(string.Format(EventoEnviadoFormat, typeof(T).Name, subscriberUrl));
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(ErroAoEnviarEventoFormat, typeof(T).Name, subscriberUrl, ex.Message));
                    return;
                }
            }
        }
        else
        {
            _logger.LogWarning(string.Format(NenhumInscritoEncontradoFormat, typeof(T).Name));
        }
    }

    private void AtualizarCache()
    {
        var serializedSubscribers = JsonConvert.SerializeObject(_subscribers[typeof(T).Name]);
        _cache.SetString(_cacheKey, serializedSubscribers);
    }
}

