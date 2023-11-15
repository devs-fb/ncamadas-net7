namespace ModeloSimples.Service.API.Teste.Integrado;

using Microsoft.Data.SqlClient;

public class TestFixture : IAsyncLifetime
{
    private const string ConnectionString = "Data Source=127.0.0.1,1434;initial catalog=master;User ID=sa;Password=!QAZ2wsx12;TrustServerCertificate=true;MultipleActiveResultSets=True;App=AppPrincipal";
    private const string DatabaseName = "ModeloSimplesTest";

    public async Task InitializeAsync()
    {
        await DropDatabaseIfExists();
    }

    public async Task DisposeAsync()
    {
        await DropDatabaseIfExists();
    }

    private static async Task DropDatabaseIfExists()
    {
        using var connection = new SqlConnection(ConnectionString);
        
        await connection.OpenAsync();

        var killConnectionsSql = $@"
            USE master;
            DECLARE @kill varchar(8000) = '';
            SELECT @kill = @kill + 'KILL ' + CONVERT(varchar(5), session_id) + ';'
              FROM sys.dm_exec_sessions
             WHERE database_id  = db_id('{DatabaseName}')
            EXEC(@kill);";

        var killCommand = new SqlCommand(killConnectionsSql, connection);
        await killCommand.ExecuteNonQueryAsync();

        var sql = $@"
                IF EXISTS(SELECT 1 FROM sys.databases WHERE name = '{DatabaseName}')
                BEGIN
                    ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{DatabaseName}];
                END";

        var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
