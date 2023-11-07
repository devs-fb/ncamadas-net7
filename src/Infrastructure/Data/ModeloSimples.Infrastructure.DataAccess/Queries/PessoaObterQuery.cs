namespace ModeloSimples.Infrastructure.DataAccess.Queries;

using Dapper;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;
using System.Data;
using System.Text;

public class PessoaObterQuery : IPessoaObterQuery
{
    private const string MESSAGEQUERYEXECUTE = "Query Execute: {0}";

    private readonly IDbConnection _dbConnection;
    private readonly ILogger<PessoaObterQuery> _logger;

    public PessoaObterQuery(IDbConnection dbConnection, ILogger<PessoaObterQuery> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultadoConsulta<PessoaModel>> ObterPessoaAsync(Guid pessoaId)
    {
        try
        {
            DynamicParameters parameters = new();
            StringBuilder sql = new();

            SQLBase(sql);

            sql.Append("AND [Pessoa].[PessoaId] = @PessoaId ");
            parameters.Add("PessoaId", pessoaId);

            _logger.LogInformation(MESSAGEQUERYEXECUTE, sql);

            var pessoas = await _dbConnection.QueryAsync<PessoaModel, PessoaFisicaModel, PessoaJuridicaModel, PessoaModel>(
                sql: sql.ToString(),
                map: (pessoa, pessoaFisica, pessoaJuridica) =>
                {
                    pessoa.PessoaFisica = pessoaFisica.NomeSocial is not null ? pessoaFisica : null;
                    pessoa.PessoaJuridica = pessoaJuridica.RazaoSocial is not null ? pessoaJuridica : null;
                    return pessoa;
                },
                param: parameters,
                splitOn: "PessoaFisicaId, PessoaJuridicaId");

            var resultadoConsulta = ResultadoConsulta<PessoaModel>.Sucesso(dados: pessoas.FirstOrDefault(), null);

            return resultadoConsulta;
        }
        catch (Exception ex)
        {
            var message = $"Erro ao obter pessoa com ID: {pessoaId}\n";
            _logger.LogError(ex, message: message);

            return ResultadoConsulta<PessoaModel>.Falha(message);
        }
    }

    private static void SQLBase(StringBuilder sql)
    {
        sql.Append(@"SELECT [Pessoa].[PessoaId]
                           ,CASE WHEN [Pessoa].[Tipo] = 'PessoaJuridica' THEN 'J'
                                 WHEN [Pessoa].[Tipo] = 'PessoaFisica' THEN 'F'
                                 ELSE '' -- quando não for nem PessoaFisica nem PessoaJuridica
                            END AS Tipo
                           ,[PessoaFisica].[PessoaFisicaId]
                           ,[PessoaFisica].[NomeSocial]
                           ,[PessoaFisica].[Genero]
                           ,[PessoaFisica].[DataNascimento]
                           ,[PessoaJuridica].[PessoaJuridicaId]
                           ,[PessoaJuridica].[RazaoSocial]
                           ,[PessoaJuridica].[NomeFantasia]
                           ,[PessoaJuridica].[CNAE]
                       FROM [ModeloSimples].[dbo].[Pessoa]
                       LEFT JOIN [ModeloSimples].[dbo].[PessoaFisica] ON [PessoaFisica].[PessoaFisicaId] = [Pessoa].[PessoaId]
                       LEFT JOIN [ModeloSimples].[dbo].[PessoaJuridica] ON [PessoaJuridica].[PessoaJuridicaId] = [Pessoa].[PessoaId]
                      WHERE [Pessoa].[Removido] = 0 
                        AND [Pessoa].[Bloquado] = 0 ");
    }
}