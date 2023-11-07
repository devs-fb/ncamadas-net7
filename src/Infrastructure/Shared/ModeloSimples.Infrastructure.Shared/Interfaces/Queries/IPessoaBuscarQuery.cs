namespace ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

public interface IPessoaBuscarQuery
{
    Task<ResultadoConsulta<IEnumerable<PessoaModel>>> BuscarPessoasAsync(DataInfo paginacaoOrdenacao, PessoaModel pessoa);
}
