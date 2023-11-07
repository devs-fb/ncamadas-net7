using ModeloSimples.Application.EventBus.Consumers;

public static class DIApplicationEventConsumer
{
    public static IServiceCollection AddApplicationEventConsumer(this IServiceCollection services) 
    {
        services.AddApplicationEventConsumerPessoa();
        
        return services;
    }

    private static IServiceCollection AddApplicationEventConsumerPessoa(this IServiceCollection services)
    {
        services.AddScoped<PessoaCriadaEventConsumer>();
        services.AddScoped<PessoaFisicaCriadaEventConsumer>();
        services.AddScoped<PessoaJuridicaCriadaEventConsumer>();
        services.AddScoped<PessoaEditadaEventConsumer>();
        services.AddScoped<PessoaFisicaEditadaEventConsumer>();
        services.AddScoped<PessoaJuridicaEditadaEventConsumer>();

        return services;
    }
}
