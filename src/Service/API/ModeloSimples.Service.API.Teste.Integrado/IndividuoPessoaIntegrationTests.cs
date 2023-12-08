namespace ModeloSimples.Service.API.Teste.Integrado;

using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text;

public class IndividuoPessoaIntegrationTests : IClassFixture<ApplicationFactory<Program>>
{
    private readonly ApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public IndividuoPessoaIntegrationTests(ApplicationFactory<Program> factory)
    {
        //Assista em: https://youtu.be/i3DPbA3N8_E?si=n5exRXdsAfWH0Fr0 para conhecer melhor
        //Ver em: https://learn.microsoft.com/pt-br/aspnet/core/test/integration-tests?view=aspnetcore-7.0

        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions() 
        {
            BaseAddress = new Uri("https://localhost:21001")
        });
    }

    [Fact]
    public async Task TestCriarPessoa()
    {
        // Arrange
        var pessoaModel = new
        {
            tipo = "F", 
            pessoaFisica = new
            {
                nomeSocial = "NomeSocial",
                dataNascimento = "2023-01-01T00:00:00Z",
                genero = "M"
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(pessoaModel), Encoding.UTF8, "application/json");

        // Act - descomente para ocorrer o teste
        //var response = await _client.PostAsync("/api/Pessoa/Criar", content);

        // Assert - descomente
        //response.EnsureSuccessStatusCode();
    }
}

