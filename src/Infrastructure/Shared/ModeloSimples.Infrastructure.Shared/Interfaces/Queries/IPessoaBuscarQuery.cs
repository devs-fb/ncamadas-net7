namespace ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

/// <summary>
/// Define uma interface para a busca assíncrona de pessoas.
/// </summary>
public interface IPessoaBuscarQuery
{
    /// <summary>
    /// Realiza uma busca assíncrona de pessoas com base em critérios fornecidos.
    /// </summary>
    /// <param name="paginacaoOrdenacao">Informações de paginação e ordenação.</param>
    /// <param name="pessoa">O modelo de pessoa contendo os critérios de busca.</param>
    /// <returns>Um resultado contendo a busca de pessoas conforme os critérios fornecidos.</returns>
    Task<ResultadoConsulta<IEnumerable<PessoaModel>>> BuscarPessoasAsync(DataInfo paginacaoOrdenacao, PessoaModel pessoa);
}