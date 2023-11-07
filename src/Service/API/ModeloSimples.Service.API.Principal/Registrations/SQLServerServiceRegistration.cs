using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModeloSimples.Infrastructure.DataAccess;
using System.Data;

public static class SQLServerServiceRegistration
{
    public static IServiceCollection AddSQLServer(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlServerPrincipalConnectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<PrincipalContext>(options =>
        {
            options.UseSqlServer(sqlServerPrincipalConnectionString);
            options.LogTo(Console.WriteLine, LogLevel.Error);
        });

        services.AddScoped<IDbConnection>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DefaultConnection"); 
            return new SqlConnection(connectionString);
        });

        return services; 
    }
}
