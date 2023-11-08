namespace ModeloSimples.Infrastructure.Shared.Interfaces;

public interface IWebhook<T>
{
    void AdicionarInscrito(string webhookUrl);
    void RemoverInscrito(string webhookUrl);
    Task EnviarEventoAsync(T evento);
}
