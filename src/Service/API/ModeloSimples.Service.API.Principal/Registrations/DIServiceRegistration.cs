using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModeloSimples.Application.Behaviors;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Domain.Services;
using ModeloSimples.Infrastructure.DataAccess;
using ModeloSimples.Infrastructure.DataAccess.Queries;
using ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;
using ModeloSimples.Infrastructure.Integration.ServiceOut.Service.Http;
using ModeloSimples.Infrastructure.Integration.ServiceOut.Service.Webhook;
using ModeloSimples.Infrastructure.Shared.Interfaces;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

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

        builder.Services.AddAutoMapper(mappingProfiles.ToArray());

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();
        builder.Services.AddScoped(typeof(IWebhook<>), typeof(WebhookHandler<>));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        services.Configure<CachingBehaviorConfiguration>(configuration.GetSection(ConstantSection.CACHINGBEHAVIORCONFIGURATION));

        return builder;
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
        services.AddSwaggerGen();

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
}


