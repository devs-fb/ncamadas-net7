public static class LogServiceRegistration
{
    public static IServiceCollection AddLogger(this IServiceCollection services, IConfiguration configuration)
    {
        var loggingConfiguration = configuration.GetSection(ConstantSection.LOGGING);

        services.AddLogging(cfgLog =>
        {
            cfgLog.AddConfiguration(loggingConfiguration);
            cfgLog.AddConsole();
        });

        return services;
    }
}
