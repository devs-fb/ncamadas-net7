using ModeloSimples.Service.API.Principal.Configurations;

public static class WebApplicationExtension
{
    public static WebApplication RegisterAllApp(this WebApplication app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.RunDevelopment();

        app.ConfigureSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.ConfigureCors();

        app.MapControllers();

        return app;
    }

    public static void ConfigureSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => { SwaggerConfiguration.ConfigureSwaggerUI(app, options); });
    }

    public static void ConfigureCors(this WebApplication app)
    {
        app.UseCorsPolicy();
    }

    public static void RunDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
        }
    }
}

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ExecuteBuilder(this WebApplicationBuilder buider)
    {

        return buider;
    }
}