using MassTransit;
using ModeloSimples.Application.EventBus;
using ModeloSimples.Service.API.Principal.Configurations;
using RabbitMQ.Client;

public class MassTransitConsumerConfig
{
    public string ConsumerName { get; set; }
    public string EventName { get; set; }
    public int RetryCount { get; set; }
    public int RetryInterval { get; set; }
    public bool Durable { get; set; }
}

public static class MassTransitServiceRegistration
{
    private const string Local_APIPrincipal = "_APIPrincipal";
    private const string ConfigurationConsumers = "RabbitMqConfiguration:Consumers";

    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {

        var rabbitMqConfiguration = configuration.GetSection(ConstantSection.RABBITMQ).Get<RabbitMqConfiguration>();
        var consumerConfigs = configuration.GetSection(ConfigurationConsumers).Get<List<MassTransitConsumerConfig>>();

        services.AddMassTransit(cfgBus =>
        {
            cfgBus.AddLogging(cfgLog =>
            {
                cfgLog.AddConfiguration(configuration);
                cfgLog.AddConsole();
            });

            cfgBus.UsingRabbitMq((context, cfgRabbit) =>
            {
                cfgRabbit.Host(new Uri($"amqps://{rabbitMqConfiguration.HostName}:{rabbitMqConfiguration.Port}/{rabbitMqConfiguration.VirtualHost}"), h =>
                {
                    h.Username(rabbitMqConfiguration.UserName);
                    h.Password(rabbitMqConfiguration.Password);
                    h.UseSsl(ssl =>
                    {
                        ssl.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                    });
                    h.Heartbeat(10);
                });

                cfgRabbit.ExchangeType = ExchangeType.Fanout;

                foreach (var consumerConfig in consumerConfigs)
                {
                    var consumerType = Type.GetType(consumerConfig.ConsumerName);
                    if (consumerType != null)
                    {
                        ConfigureReceiveEndpoint(cfgRabbit, context, Local_APIPrincipal, consumerConfig.EventName, consumerConfig.RetryCount, consumerConfig.RetryInterval, consumerConfig.Durable, consumerType);
                    }
                }
            });

        });

        return services;
    }

    private static void ConfigureReceiveEndpoint(
    IRabbitMqBusFactoryConfigurator configurator,
    IRegistrationContext context,
    string eventLocal,
    string eventName,
    int retryCount,
    int interval,
    bool durable,
    Type consumerType)
    {
        var method = typeof(MassTransitServiceRegistration)
            .GetMethod(nameof(ConfigureReceiveEndpoint), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?.MakeGenericMethod(consumerType);

        method?.Invoke(null, new object[] { configurator, context, eventLocal, eventName, retryCount, interval, durable });
    }

    private static void ConfigureReceiveEndpoint<TConsumer>(IRabbitMqBusFactoryConfigurator configurator, IRegistrationContext context, string eventLocal, string eventName, (int retryCount, int interval)? messageRetry = null, bool durable = true)
    where TConsumer : class, IConsumer
    {
        var (retryCount, interval) = messageRetry ?? (2, 1000);

        configurator.ReceiveEndpoint($"{eventName}{eventLocal}", ep =>
        {
            ep.UseMessageRetry(x => x.Interval(retryCount, interval));
            ep.Durable = true;
            ep.Bind(eventName);
            ep.Consumer<TConsumer>(context);
            ep.Consumer<PublishErrorHandler>(context);
        });
    }
}
