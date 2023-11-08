namespace ModeloSimples.Infrastructure.Shared.Interfaces;

public interface IHttpClientService
{
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
}