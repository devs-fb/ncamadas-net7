namespace ModeloSimples.Infrastructure.DataAccess.Queries;

using Dapper;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;
using Newtonsoft.Json;
using System.Data;
using System.Text;

public class PessoaBuscarQuery : IPessoaBuscarQuery
{
    private const string MESSAGEQUERYEXECUTE = "Query Execute: {0}";
    private const string ERRORMENSSAGE = "Erro ao buscar pessoas. \n {0} \n\n Mensagem do Erro: {1}";

    private readonly IDbConnection _dbConnection;
    private readonly ILogger<PessoaBuscarQuery> _logger;

    public PessoaBuscarQuery(IDbConnection dbConnection, ILogger<PessoaBuscarQuery> logger)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ResultadoConsulta<IEnumerable<PessoaModel>>> BuscarPessoasAsync(DataInfo paginacaoOrdenacao, PessoaModel pessoa)
    {
        try
        {
            if (pessoa is null)
                throw new ArgumentNullException(nameof(paginacaoOrdenacao));

            paginacaoOrdenacao ??= new DataInfo(new PaginacaoInfo(1, 10, 0), new List<OrdenacaoInfo>());

            DynamicParameters parameters = new();
            StringBuilder sql = new();

            SQLBase(sql);

            QualificarConsutaPorTipoDePessoa(parameters, sql, pessoa);

            AdicionarClausulasOrderBy(paginacaoOrdenacao, sql);

            // Paginação
            var pagina = paginacaoOrdenacao.Paginacao?.Pagina ?? 1;
            var tamanhoPagina = paginacaoOrdenacao.Paginacao?.TamanhoPagina ?? 10;
            var offset = (pagina - 1) * tamanhoPagina;

            if (offset > 1)
                sql.Append($" OFFSET {offset} ROWS FETCH NEXT {tamanhoPagina} ROWS ONLY");

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
            
            var totalRegistros = await SQLCount();

            var resultadoConsulta = ResultadoConsulta<IEnumerable<PessoaModel>>
                .Sucesso(
                dados: pessoas,
                paginacaoOrdenacao: new DataInfo(
                    paginacao: new PaginacaoInfo(pagina, tamanhoPagina, totalRegistros),
                    ordenacao: paginacaoOrdenacao.Ordenacao));

            return resultadoConsulta;
        }
        catch (Exception ex) 
        {
            var objetoserializado = JsonConvert.SerializeObject(pessoa);
            var message = string.Format(ERRORMENSSAGE, objetoserializado, ex.Message);
            
            _logger.LogError(ex, message: message);

            return ResultadoConsulta<IEnumerable<PessoaModel>>.Falha(message);
        }
    }

    private static void QualificarConsutaPorTipoDePessoa(DynamicParameters parameters, StringBuilder sql, PessoaModel pessoa)
    {
        if (TipoDePessoaValida(pessoa.Tipo))
        {
            switch (pessoa.Tipo)
            {
                case "F":
                    sql.Append("AND [Pessoa].[Tipo] = @Tipo ");
                    parameters.Add("Tipo", "PessoaFisica");
                    AdicionarClausulaWherePessoaFisica(sql, pessoa.PessoaFisica, parameters); 
                    break;
                case "J": 
                    AdicionarClausulaWherePessoaJuridica(sql, pessoa.PessoaJuridica, parameters);
                    sql.Append("AND [Pessoa].[Tipo] = @Tipo ");
                    parameters.Add("Tipo", "PessoaFisica");
                    break;
                default: AdicionarClausulaWherePessoaIndefinida(sql, pessoa, parameters); break;
            }
        }
    }

    private static bool TipoDePessoaValida(string tipo)
        => !string.IsNullOrWhiteSpace(tipo) && (!tipo.Equals("F") || !tipo.Equals("J"));

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

    private async Task<int> SQLCount()
    {
        var sql = @"SELECT COUNT(*) 
                      FROM [ModeloSimples].[dbo].[Pessoa] 
                      LEFT JOIN [ModeloSimples].[dbo].[PessoaFisica] 
                        ON [PessoaFisica].[PessoaFisicaId] = [Pessoa].[PessoaId]
                      LEFT JOIN [ModeloSimples].[dbo].[PessoaJuridica] 
                        ON [PessoaJuridica].[PessoaJuridicaId] = [Pessoa].[PessoaId]
                     WHERE [Pessoa].[Removido] = 0 
                       AND [Pessoa].[Bloquado] = 0 ";

        return await _dbConnection
            .ExecuteScalarAsync<int>(sql);
    }

    private static void AdicionarClausulaWherePessoaIndefinida(StringBuilder sql, PessoaModel pessoa, DynamicParameters parameters)
    {
        AdicionarClausulaWherePessoaJuridica(sql, pessoa.PessoaJuridica, parameters);
        AdicionarClausulaWherePessoaFisica(sql, pessoa.PessoaFisica, parameters); 
    }

    private static void AdicionarClausulaWherePessoaFisica(StringBuilder sql, PessoaFisicaModel pessoaFisica, DynamicParameters parameters)
    {
        if (!string.IsNullOrEmpty(pessoaFisica.NomeSocial))
        {
            sql.Append("AND [PessoaFisica].[NomeSocial] LIKE @NomeSocial ");
            parameters.Add("NomeSocial", $"%{pessoaFisica.NomeSocial}%");
        }
    }

    private static void AdicionarClausulaWherePessoaJuridica(StringBuilder sql, PessoaJuridicaModel pessoaJuridica, DynamicParameters parameters)
    {
        if (!string.IsNullOrEmpty(pessoaJuridica.RazaoSocial))
        {
            sql.Append("AND [PessoaJuridica].[RazaoSocial] LIKE @RazaoSocial ");
            parameters.Add("RazaoSocial", $"%{pessoaJuridica.RazaoSocial}%");
        }

        // Adicione outras cláusulas WHERE para PessoaJuridica conforme necessário
        // ...
    }

    private static void AdicionarClausulasOrderBy(DataInfo paginacaoOrdenacao, StringBuilder sql)
    {
        if (paginacaoOrdenacao.Ordenacao != null && paginacaoOrdenacao.Ordenacao.Any())
        {
            var orderClauses = paginacaoOrdenacao.Ordenacao
                .Select(o => $"[{o.Campo}] {(o.Ascendente ? "ASC" : "DESC")}")
                .ToList();

            sql.Append("ORDER BY ");
            sql.Append(string.Join(", ", orderClauses));
        }
    }
}
