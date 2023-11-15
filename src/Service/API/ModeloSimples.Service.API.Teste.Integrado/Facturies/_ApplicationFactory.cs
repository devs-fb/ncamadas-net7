namespace ModeloSimples.Service.API.Teste.Integrado.Facturies;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    public ApplicationFactory()
    {
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {

        });

        builder.UseEnvironment("IntegrationTests");

        base.ConfigureWebHost(builder);
    }
}