using ModeloSimples.Service.API.Principal.Configurations;

public static class StackExchangeRedisCacheServiceRegistration
{
    public static IServiceCollection AddStackExchangeRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConfig = configuration.GetSection(ConstantSection.REDIS).Get<RedisConfiguration>();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConfig.ConnectionString;
            options.InstanceName = redisConfig.InstanceName;
        });

        return services;
    }
}
