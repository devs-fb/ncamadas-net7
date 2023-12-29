namespace ModeloSimples.Infrastructure.Shared.DTO.CommandQuery.LGPD;

using MediatR;
using ModeloSimples.Infrastructure.Shared.Common;

/// <summary>
/// Define uma solicitação de busca de pessoas com informações de paginação e ordenação.
/// </summary>
public interface IPessoasBuscarCommandQuery : IRequest<ResultadoConsulta<IEnumerable<PessoaModel>>>
{
    /// <summary>
    /// Obtém ou define as informações de paginação e ordenação para a busca.
    /// </summary>
    DataInfo PaginacaoOrdenacao { get; set; }

    /// <summary>
    /// Obtém ou define o modelo de pessoa para filtrar a busca.
    /// </summary>
    PessoaModel Pessoa { get; set; }
}
