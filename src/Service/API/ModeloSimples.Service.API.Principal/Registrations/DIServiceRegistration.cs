using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ModeloSimples.Application.Behaviors;
using ModeloSimples.Application.Queries;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Domain.Services;
using ModeloSimples.Infrastructure.DataAccess;
using ModeloSimples.Infrastructure.DataAccess.Queries;
using ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;
using ModeloSimples.Infrastructure.Integration.ServiceOut.Service.Http;
using ModeloSimples.Infrastructure.Integration.ServiceOut.Service.Webhook;
using ModeloSimples.Infrastructure.Shared.DTO.CommandQuery.LGPD;
using ModeloSimples.Infrastructure.Shared.Interfaces;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;
using ModeloSimples.Service.API.Principal.Common;
using ModeloSimples.Service.API.Principal.Configurations;

public static class DIServiceRegistration
{
    public static WebApplicationBuilder RegisterAllServices(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;

        DapperConfig.ConfigureMappings();

        services.RegistrationAllServices(configuration, builder.Environment);

        var mappingProfiles = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract);

        services.AddAutoMapper(mappingProfiles.ToArray());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddHttpClient<IHttpClientService, HttpClientService>();

        services.AddScoped(typeof(IWebhook<>), typeof(WebhookHandler<>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        services.Configure<CachingBehaviorConfiguration>(configuration.GetSection(ConstantSection.CACHINGBEHAVIORCONFIGURATION));

        services.AddAddHealthChecksService(configuration);

        return builder;
    }

    public static void AddApiVersion(this IServiceCollection service)
    {
        //service.AddApiVersioning(config =>
        //{
        //    config.DefaultApiVersion = new ApiVersion(1, 0);
        //    config.AssumeDefaultVersionWhenUnspecified = true;
        //    config.ReportApiVersions = true;
        //});
    }

    private static IServiceCollection RegistrationAllServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddLogger(configuration);
        services.AddStackExchangeRedisCache(configuration);
        services.AddSQLServer(configuration, environment);
        services.AddMassTransit(configuration);

        services.AddApplication();
        services.AddDomain();
        services.AddInfrastructure();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => { SwaggerConfiguration.ConfigureSwaggerGen(options, configuration); });

        return services;
    }

    private static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddApplicationEvent();

        return services;
    }

    private static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPessoaService), typeof(PessoaService));

        return services;
    }

    private static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddInfrastructureShared();
        services.AddInfrastructureDataAccess();

        return services;
    }

    private static IServiceCollection AddInfrastructureShared(this IServiceCollection services)
    {
        services.AddScoped(typeof(IPessoasBuscarCommandQuery), typeof(PessoasBuscarCommandQuery));

        return services;
    }

    private static IServiceCollection AddInfrastructureDataAccess(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

        services.AddScoped(typeof(IPessoaBuscarQuery), typeof(PessoaBuscarQuery));
        services.AddScoped(typeof(IPessoaObterQuery), typeof(PessoaObterQuery));
        services.AddScoped(typeof(IPessoaAuditarQuery), typeof(PessoaAuditarQuery));        

        return services;
    }

    private static IServiceCollection AddAddHealthChecksService(this IServiceCollection services, ConfigurationManager configuration)
    {
        const string AMQP = "amqps://{0}:{1}/{2}";
        
        var rabbitMqConfiguration = configuration.GetSection(ConstantSection.RABBITMQ).Get<RabbitMqConfiguration>();
        var rabbitConnectionString = string.Format(AMQP, rabbitMqConfiguration.HostName, rabbitMqConfiguration.Port, rabbitMqConfiguration.VirtualHost);
        var redisConfig = configuration.GetSection(ConstantSection.REDIS).Get<RedisConfiguration>();

        services
            .AddHealthChecks()
                .AddSqlServer(
                    connectionString: configuration.GetConnectionString(ConstantGlobal.StringConnectionName),
                    healthQuery: ConstantHealthChecks.HealthQuery,
                    name: ConstantHealthChecks.PrincipalContext,
                    failureStatus: HealthStatus.Degraded,
                    tags: new string[]
                    {                        
                        ConstantHealthChecks.SQLServer,
                        ConstantHealthChecks.DataBase
                    })
                .AddRabbitMQ(
                    rabbitConnectionString: rabbitConnectionString,
                    name: "rabbitmq-broker",
                    tags: new string[] { "rabbitmq", "broker" })
                .AddRedis(
                    redisConnectionString: redisConfig.ConnectionString,
                    name: "redis-cache",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "redis", "cache" })
                .AddCheck(
                    ConstantHealthChecks.Self,
                    () => HealthCheckResult.Healthy(),
                    tags: new[] { ConstantHealthChecks.Self });


                //.AddDatadogPublisher(ConstantHealthChecks.DatadogPublisherName);

        services
            .AddHealthChecksUI(setupSettings: setup => 
            {
                setup.AddHealthCheckEndpoint("SampleCheck", "/hc");
                setup.MaximumHistoryEntriesPerEndpoint(500);
                setup.SetMinimumSecondsBetweenFailureNotifications(4);
                setup.SetEvaluationTimeInSeconds(5);
                setup.SetApiMaxActiveRequests(5);
            })
            .AddInMemoryStorage();

        return services;
    }
}


