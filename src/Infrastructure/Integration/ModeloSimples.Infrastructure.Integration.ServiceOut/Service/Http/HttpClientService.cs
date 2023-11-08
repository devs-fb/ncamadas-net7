namespace ModeloSimples.Infrastructure.Integration.ServiceOut.Service.Http;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        return await _httpClient.PostAsync(requestUri, content);
    }
}
