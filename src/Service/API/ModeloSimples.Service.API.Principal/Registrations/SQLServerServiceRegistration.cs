using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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

        services.AddHealthChecks().AddCheck("sql-server-check", new SQLServerHealthCheck(sqlServerPrincipalConnectionString));

        return services;
    }
}

public class SQLServerHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    private const string ConnectionUnavailableMessage = "Não foi possível abrir uma conexão com o SQL Server.";
    private const string ExceptionMessage = "Ocorreu uma exceção ao verificar a conexão com o SQL Server.";

    public SQLServerHealthCheck(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            if (connection.State == ConnectionState.Open)
            {
                return HealthCheckResult.Healthy();
            }
            else
            {
                return HealthCheckResult.Unhealthy(ConnectionUnavailableMessage);
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(ExceptionMessage, ex);
        }
    }
}