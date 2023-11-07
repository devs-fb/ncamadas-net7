namespace ModeloSimples.Infrastructure.DataAccess.Queries;

using Dapper;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;
using System.Data;
using System.Text;

public class PessoaAuditarQuery : IPessoaAuditarQuery
{
    private const string MESSAGEQUERYEXECUTE = "Query Execute: {0}";

    private readonly IDbConnection _dbConnection;
    private readonly ILogger<PessoaAuditarQuery> _logger;

    public PessoaAuditarQuery(IDbConnection dbConnection, ILogger<PessoaAuditarQuery> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultadoConsulta<List<PessoaAuditadaModel>>> AuditarPessoaAsync(Guid Id)
    {
        try
        {
            DynamicParameters parameters = new();
            StringBuilder sql = new();

            SQLBase(sql);

            sql.Append("AND P.[PessoaId] = @Id ");
            parameters.Add("Id", Id);

            _logger.LogInformation(MESSAGEQUERYEXECUTE, sql);

            var pessoas = await _dbConnection.QueryAsync<PessoaAuditadaModel, PessoaFisicaAuditadaModel, PessoaJuridicaAuditadaModel, PessoaAuditadaModel>(
                sql: sql.ToString(),
                map: (pessoa, pessoaFisica, pessoaJuridica) =>
                {
                    if (pessoa.Tipo == "F")
                    {
                        pessoa.PessoaFisica = pessoaFisica;
                    }
                    else if (pessoa.Tipo == "J")
                    {
                        pessoa.PessoaJuridica = pessoaJuridica;
                    }
                    return pessoa;
                },
                param: parameters,
                splitOn: "PessoaFisicaId, PessoaJuridicaId");

            var resultadoConsulta = ResultadoConsulta<List<PessoaAuditadaModel>>.Sucesso(dados: pessoas.ToList() , null);

            return resultadoConsulta;
        }
        catch (Exception ex)
        {
            var message = $"Erro ao auditar pessoa com ID: {Id}\n";
            _logger.LogError(ex, message: message);

            return ResultadoConsulta<List<PessoaAuditadaModel>>.Falha(message);
        }
    }

    private static void SQLBase(StringBuilder sql)
    {
        sql.Append(@"
        SELECT CONVERT(UNIQUEIDENTIFIER, JSON_VALUE(P.[Versao], '$[0].PessoaId')) AS Id
              ,CASE 
                   WHEN JSON_VALUE(P.[Versao], '$[0].Tipo') = 'PessoaJuridica' THEN 'J'
                   WHEN JSON_VALUE(P.[Versao], '$[0].Tipo') = 'PessoaFisica' THEN 'F'
                   ELSE '' 
               END AS Tipo
              ,CONVERT(datetime2, 
                   CASE 
                       WHEN JSON_VALUE(P.[Versao], '$[0].Controle.Modified') IS NOT NULL 
                       THEN JSON_VALUE(P.[Versao], '$[0].Controle.Modified')
                       ELSE JSON_VALUE(P.[Versao], '$[0].Controle.Created')
                   END, 127) AS Operacao
              ,JSON_VALUE(P.[Versao], '$[0].Controle.IsRemoved') AS Removido
              ,JSON_VALUE(P.[Versao], '$[0].Controle.IsBlocked') AS Bloqueado
              ,JSON_VALUE(P.[Versao], '$[0].PessoaFisica.PessoaFisicaId') AS PessoaFisicaId
              ,JSON_VALUE(P.[Versao], '$[0].PessoaFisica.NomeSocial') AS NomeSocial
              ,JSON_VALUE(P.[Versao], '$[0].PessoaFisica.DataNascimento') AS DataNascimento
              ,JSON_VALUE(P.[Versao], '$[0].PessoaFisica.Genero') AS Genero
              ,JSON_VALUE(P.[Versao], '$[0].PessoaJuridica.PessoaJuridicaId') AS PessoaJuridicaId
              ,JSON_VALUE(P.[Versao], '$[0].PessoaJuridica.RazaoSocial') AS RazaoSocial
              ,JSON_VALUE(P.[Versao], '$[0].PessoaJuridica.NomeFantasia') AS NomeFantasia
              ,JSON_VALUE(P.[Versao], '$[0].PessoaJuridica.CNAE') AS CNAE
          FROM [ModeloSimples].[dbo].[Pessoa] P
         WHERE ISJSON(P.[Versao]) > 0 ");
    }
}