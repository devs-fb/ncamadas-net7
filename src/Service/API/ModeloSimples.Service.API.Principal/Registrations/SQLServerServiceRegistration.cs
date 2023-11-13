using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModeloSimples.Infrastructure.DataAccess;
using System.Data;

public static class SQLServerServiceRegistration
{
    private const string StringConnectionName = "DefaultConnection";
    public static IServiceCollection AddSQLServer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var sqlServerPrincipalConnectionString = configuration.GetConnectionString(StringConnectionName);

        services.AddDbContext<PrincipalContext>(options =>
        {           
            options.UseSqlServer(sqlServerPrincipalConnectionString);
            options.LogTo(Console.WriteLine, LogLevel.Error);
        });

        services.AddScoped<IDbConnection>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connection = new SqlConnection(sqlServerPrincipalConnectionString);
            
            return connection;
        });

        return services;
    }
}
