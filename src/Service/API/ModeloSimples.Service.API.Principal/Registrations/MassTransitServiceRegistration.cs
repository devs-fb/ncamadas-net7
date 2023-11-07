using MassTransit;
using ModeloSimples.Application.EventBus;
using ModeloSimples.Application.EventBus.Consumers;
using ModeloSimples.Service.API.Principal.Configurations;
using RabbitMQ.Client;

public static class MassTransitServiceRegistration
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConfiguration = configuration.GetSection(ConstantSection.RABBITMQ).Get<RabbitMqConfiguration>();

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
                    h.Heartbeat(120);
                });

                cfgRabbit.ExchangeType = ExchangeType.Fanout;

                cfgRabbit.ReceiveEndpoint("PessoaCriada_APIPrincipal", ep =>
                {
                    ep.UseMessageRetry(x => x.Interval(2, 1000));
                    ep.Durable = true;
                    ep.Bind("PessoaCriada");
                    ep.Consumer<PessoaCriadaEventConsumer>(context);
                    ep.Consumer<PublishErrorHandler>(context);
                });

                cfgRabbit.ReceiveEndpoint("PessoaFisicaCriada_APIPrincipal", ep =>
                {
                    ep.UseMessageRetry(x => x.Interval(2, 1000));
                    ep.Durable = true;
                    ep.Bind("PessoaJuridicaCriada");
                    ep.Consumer<PessoaFisicaCriadaEventConsumer>(context);
                    ep.Consumer<PublishErrorHandler>(context);
                });

                cfgRabbit.ReceiveEndpoint("PessoaJuridicaCriada_APIPrincipal", ep =>
                {
                    ep.UseMessageRetry(x => x.Interval(2, 1000));
                    ep.Durable = true;
                    ep.Bind("PessoaJuridicaCriada");
                    ep.Consumer<PessoaJuridicaCriadaEventConsumer>(context);
                    ep.Consumer<PublishErrorHandler>(context);
                });

                cfgRabbit.ReceiveEndpoint("PessoaEditada_APIPrincipal", ep =>
                {
                    ep.UseMessageRetry(x => x.Interval(2, 1000));
                    ep.Durable = true;
                    ep.Bind("PessoaEditada");
                    ep.Consumer<PessoaEditadaEventConsumer>(context);
                    ep.Consumer<PublishErrorHandler>(context);
                });

                cfgRabbit.ReceiveEndpoint("PessoaFisicaEditada_APIPrincipal", ep =>
                {
                    ep.UseMessageRetry(x => x.Interval(2, 1000));
                    ep.Durable = true;
                    ep.Bind("PessoaJuridicaEditada");
                    ep.Consumer<PessoaFisicaEditadaEventConsumer>(context);
                    ep.Consumer<PublishErrorHandler>(context);
                });

                cfgRabbit.ReceiveEndpoint("PessoaJuridicaEditada_APIPrincipal", ep =>
                {
                    ep.UseMessageRetry(x => x.Interval(2, 1000));
                    ep.Durable = true;
                    ep.Bind("PessoaJuridicaEditada");
                    ep.Consumer<PessoaJuridicaEditadaEventConsumer>(context);
                    ep.Consumer<PublishErrorHandler>(context);
                });
            });
        });

        return services;
    }
}
