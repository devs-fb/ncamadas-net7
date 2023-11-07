namespace ModeloSimples.Application.Queries;

using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

public class PessoasBuscarCommandQuery : IRequest<ResultadoConsulta<IEnumerable<PessoaModel>>>
{
    public DataInfo PaginacaoOrdenacao { get; set; }

    public PessoaModel Pessoa { get; set; }
}

public class PessoasBuscarCommandQueryHandler : IRequestHandler<PessoasBuscarCommandQuery, ResultadoConsulta<IEnumerable<PessoaModel>>>
{
    private const string ERROMESSAGE = "Erro ao buscar pessoa";

    private readonly ILogger<PessoasBuscarCommandQueryHandler> _logger;
    private readonly IPessoaBuscarQuery _buscarPessoaQuery;

    public PessoasBuscarCommandQueryHandler(ILogger<PessoasBuscarCommandQueryHandler> logger, IPessoaBuscarQuery buscarPessoaQuery)
    {
        _logger = logger;
        _buscarPessoaQuery = buscarPessoaQuery ?? throw new ArgumentNullException(nameof(buscarPessoaQuery)); ;
    }

    public async Task<ResultadoConsulta<IEnumerable<PessoaModel>>> Handle(PessoasBuscarCommandQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var paginacaoOrdenacao = request.PaginacaoOrdenacao;
            var pessoa = request.Pessoa;

            var resultadoConsulta = await _buscarPessoaQuery.BuscarPessoasAsync(paginacaoOrdenacao, pessoa);

            return resultadoConsulta;
        }
        catch (Exception ex)
        {           
            _logger.LogError(ERROMESSAGE, ex);

            return await Task.FromResult(ResultadoConsulta<IEnumerable<PessoaModel>>.Falha(ERROMESSAGE));
        }
    }
}