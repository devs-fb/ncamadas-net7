using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.FileProviders;
using ModeloSimples.Service.API.Principal.Configurations;

public static class WebApplicationExtension
{
    public static WebApplication RegisterAllApp(this WebApplication app)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(app.Environment.ContentRootPath, "Assets")), 
            RequestPath = "/img" 
        });

        app.RunDevelopment();

        app.ConfigureSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.ConfigureCors();

        app.MapControllers();

        app.UseRouting();

        app.ConfigureEndpoints();

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

    public static void ConfigureEndpoints(this WebApplication app)
    {
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapHealthChecks("/hc", new HealthCheckOptions 
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            endpoint.MapHealthChecksUI(setup =>
            {
                setup.UIPath = "/hc-ui";
                setup.AddCustomStylesheet($"{app.Environment.ContentRootPath}/Assets/HealthChecks_Dark.css");
            });
        });
    }
}

public static class WebApplicationBuilderExtension
{
    public static WebApplicationBuilder ExecuteBuilder(this WebApplicationBuilder buider)
    {

        return buider;
    }
}