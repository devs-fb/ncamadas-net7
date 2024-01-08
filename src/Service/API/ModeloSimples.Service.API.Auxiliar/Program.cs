using Hangfire;
using Hangfire.MemoryStorage;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;

const int ZERO = 0;
const int VINTEMIL = 20000;
const string PayloadReceivedMessage = "Recebido payload para o webhook '{0}': {1}";
const string PayloadReceivedMessageFormat = "Serviço: '{0}' Resultado: {1}";
const string SuccessResponseMessage = "Payload para o webhook '{0}' recebido com sucesso!";
const string ErrorMessageFormat = "Erro ao processar a solicitação para o webhook '{0}': {1}";
const string HangfireConnection = "HangfireConnection";
const string HangfireAutomaticRetryAttempts = "Hangfire:AutomaticRetryAttempts";
const string AppSettings = "appsettings.json";
const string WebhookGroup = "/webhook";
const string HC = "/hc";
const string HangFire = "/hangfire";

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateSlimBuilder(args);

var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var logger = loggerFactory.CreateLogger<Program>();

builder.Services.AddSingleton<WebhookProcessor>();
builder.Services.AddHttpClient();
builder.Services.AddLogging();

var configuration = new ConfigurationBuilder()
    .AddJsonFile(AppSettings, optional: false)
    .Build();

builder.WebHost.UseKestrel(options =>
{
    options.ListenAnyIP(VINTEMIL, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(ZERO, AppJsonSerializerContext.Default);
});

builder.Services.AddHealthChecks();

var connectionString = configuration.GetConnectionString(HangfireConnection);
var automaticRetryAttempts = configuration.GetValue<int>(HangfireAutomaticRetryAttempts);

builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    config.UseMemoryStorage();
    config.UseFilter(new AutomaticRetryAttribute { Attempts = automaticRetryAttempts });
});

var app = builder.Build();

var webhookApi = app.MapGroup(WebhookGroup);

webhookApi.MapPost("/{webhookName}", async (HttpContext context, string webhookName) =>
{
    try
    {
        var payload = await context.Request.ReadFromJsonAsync<WebhookPayload>();
        
        if (payload is not null)
        {
            var payloadMessage = string.Format(PayloadReceivedMessageFormat, payload?.Servico, payload?.Status);

            logger.LogInformation(string.Format(PayloadReceivedMessage, webhookName, payloadMessage));

            BackgroundJob.Enqueue<WebhookProcessor>(processor => processor.ProcessWebhookPayload(payload));
        }

        context.Response.StatusCode = StatusCodes.Status200OK;
        await context.Response.WriteAsync(string.Format(SuccessResponseMessage, webhookName)).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        logger.LogError(string.Format(ErrorMessageFormat, webhookName, ex.Message));
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(string.Format(ErrorMessageFormat, webhookName, ex.Message)).ConfigureAwait(false);
    }
});

app.UseHealthChecks(HC);

#pragma warning disable CS0618 // O tipo ou membro é obsoleto
app.UseHangfireServer();
#pragma warning restore CS0618 

app.UseHangfireDashboard(HangFire);

app.Run();

[JsonSerializable(typeof(WebhookPayload))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

public class WebhookPayload
{
    public required string Servico { get; set; }
    public required string Uri { get; set; }
    public required string Status { get; set; }

    public string? Description { get; set; }
    public string? Timestamp { get; set; }
}

public class WebhookProcessor
{

    private const string ProcessingMessage = "{3} :: Processando payload do webhook - Serviço: {0}, Status: {1} - {2}";
    private const string ProcessedSuccessfullyMessage = "Payload do webhook processado com sucesso - Serviço: {0}, Status: {1}";
    private const string ErrorAccessingEndpointMessage = "Erro ao acessar o endpoint: {0}";
    private const string ErrorProcessingWebhookMessage = "Erro ao processar o webhook: {0}";
    private const int SleepTime = 5000;

    private readonly ILogger<WebhookProcessor> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public WebhookProcessor(ILogger<WebhookProcessor> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public void ProcessWebhookPayload(WebhookPayload? payload)
    {
        if (payload is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(payload.Description))
        {
            _logger.LogInformation(string.Format(ProcessingMessage, payload?.Servico, payload?.Status, "", DateTime.Now.ToString("yyyyMMddHHmmssfffff")));
        }
        else
        {
            _logger.LogInformation(string.Format(ProcessingMessage, payload?.Servico, payload?.Status, payload?.Description, payload?.Timestamp));
        }

        if (payload is not null)
        {
            var uri = new Uri(payload.Uri);
            var resultado = GetApiStatusAsync(uri).GetAwaiter().GetResult();

            if (resultado is not null)
            {
                var sqlserver = resultado.Entries.Where(p => p.Key == "principalcontext-db").SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(sqlserver.Key))
                {
                    var degraded = sqlserver.Value.Status.ToLowerInvariant().Equals("degraded", StringComparison.InvariantCultureIgnoreCase);
                    if (degraded)
                    {
                        CorrigirSQLServerOffLine();
                    }
                }
            }
        }
        Thread.Sleep(SleepTime);

        _logger.LogInformation(string.Format(ProcessedSuccessfullyMessage, payload?.Servico, payload?.Status));
    }

    private async Task<ApiStatus?> GetApiStatusAsync(Uri apiUri)
    {
        var httpClient = _httpClientFactory.CreateClient();

        try
        {
            var response = await httpClient.GetAsync(apiUri);

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return null;
                }


                return JsonConvert.DeserializeObject<ApiStatus>(jsonResult);
            }
            else
            {
                _logger.LogError(string.Format(ErrorAccessingEndpointMessage, response.StatusCode));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format(ErrorProcessingWebhookMessage, ex.Message));
        }

        return null;
    }

    private static void CorrigirSQLServerOffLine()
    {
        string nomeDoConteiner = "f540feb2";

        ProcessStartInfo startInfo = new()
        {
            FileName = "podman",
            Arguments = $"start {nomeDoConteiner}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = new()
        {
            StartInfo = startInfo
        };

        process.Start();
        process.WaitForExit();

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
        {
            Console.WriteLine("Saída do comando: ");
            Console.WriteLine(output);
        }

        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine("Erro ao executar o comando: ");
            Console.WriteLine(error);
        }
    }
}



public record ApiStatus(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("totalDuration")] TimeSpan TotalDuration,
    [property: JsonPropertyName("entries")] Dictionary<string, HealthEntry> Entries
);

public record HealthEntry(
    [property: JsonPropertyName("data")] HealthData Data,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("duration")] TimeSpan Duration,
    [property: JsonPropertyName("exception")] string Exception,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("tags")] List<string> Tags
);

public record HealthData(
    [property: JsonPropertyName("Endpoints")] Dictionary<string, EndpointStatus> Endpoints
);

public record EndpointStatus(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("description")] string Description
);




