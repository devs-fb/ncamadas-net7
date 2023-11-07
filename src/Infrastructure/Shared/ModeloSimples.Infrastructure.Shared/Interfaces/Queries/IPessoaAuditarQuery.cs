namespace ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

public interface IPessoaAuditarQuery
{
    Task<ResultadoConsulta<List<PessoaAuditadaModel>>> AuditarPessoaAsync(Guid Id);
}