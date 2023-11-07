using ModeloSimples.Application.EventBus;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public static class DIApplicationEvent
{
    public static IServiceCollection AddApplicationEvent(this IServiceCollection services) 
    {
        services.AddApplicationEventBus();
        services.AddApplicationEventConsumer();
        
        return services;
    }

    private static IServiceCollection AddApplicationEventBus(this IServiceCollection services)
    {
        services.AddScoped<PublishErrorHandler>();
        services.AddScoped<IEventBus, MassTransitEventBus>();

        return services;
    }
}
