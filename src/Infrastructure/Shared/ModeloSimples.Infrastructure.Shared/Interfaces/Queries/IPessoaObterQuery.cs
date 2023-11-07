namespace ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

public interface IPessoaObterQuery
{
    Task<ResultadoConsulta<PessoaModel>> ObterPessoaAsync(Guid pessoaId);
}
