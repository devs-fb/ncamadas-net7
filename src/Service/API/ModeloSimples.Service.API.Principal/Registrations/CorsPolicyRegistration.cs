using ModeloSimples.Service.API.Principal.Configurations;

public static class CorsPolicyRegistration
{
    public static IApplicationBuilder UseCorsPolicy(this WebApplication app)
    {
        var corsSettings = app.Configuration.GetSection("CorsPolicyConfiguration").Get<List<CorsPolicyConfiguration>>();

        if (corsSettings != null && corsSettings.Any())
        {
            app.UseCors(builder =>
            {
                foreach (var corsPolicy in corsSettings)
                {
                    builder.WithOrigins(corsPolicy.AllowedOrigins.ToArray());

                    if (corsPolicy.AllowedHeaders != null && corsPolicy.AllowedHeaders.Any())
                    {
                        builder.WithHeaders(corsPolicy.AllowedHeaders.ToArray());
                    }

                    if (corsPolicy.AllowedMethods != null && corsPolicy.AllowedMethods.Any())
                    {
                        builder.WithMethods(corsPolicy.AllowedMethods.ToArray());
                    }

                    if (corsPolicy.ExposedHeaders != null && corsPolicy.ExposedHeaders.Any())
                    {
                        builder.WithExposedHeaders(corsPolicy.ExposedHeaders.ToArray());
                    }
                }
            });
        }

        return app;
    }

}
