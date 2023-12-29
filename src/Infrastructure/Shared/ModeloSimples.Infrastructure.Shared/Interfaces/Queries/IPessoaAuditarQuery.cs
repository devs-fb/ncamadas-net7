namespace ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

/// <summary>
/// Define uma interface para consulta de auditoria de pessoas.
/// </summary>
public interface IPessoaAuditarQuery
{
    /// <summary>
    /// Realiza uma auditoria assíncrona de uma pessoa com base no seu identificador.
    /// </summary>
    /// <param name="Id">O identificador da pessoa.</param>
    /// <returns>Um resultado contendo a consulta de auditoria da pessoa.</returns>
    Task<ResultadoConsulta<List<PessoaAuditadaModel>>> AuditarPessoaAsync(Guid Id);
}