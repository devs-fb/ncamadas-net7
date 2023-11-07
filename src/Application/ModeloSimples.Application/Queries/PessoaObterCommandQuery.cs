namespace ModeloSimples.Application.Queries;

using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

public class PessoaObterCommandQuery : IRequest<ResultadoConsulta<PessoaModel>>
{
    public Guid Id { get; }

    public PessoaObterCommandQuery(Guid id)
    {
        Id = id;
    }
}

public class PessoaObterCommandQueryHandler : IRequestHandler<PessoaObterCommandQuery, ResultadoConsulta<PessoaModel>>
{
    private const string ERROMESSAGE = "Erro ao obter pessoa com ID: {0}\n";

    private readonly ILogger<PessoaObterCommandQueryHandler> _logger;
    private readonly IPessoaObterQuery _obterPessoaQuery;

    public PessoaObterCommandQueryHandler(ILogger<PessoaObterCommandQueryHandler> logger, IPessoaObterQuery obterPessoaQuery)
    {
        _logger = logger;
        _obterPessoaQuery = obterPessoaQuery ?? throw new ArgumentNullException(nameof(obterPessoaQuery));
    }

    public async Task<ResultadoConsulta<PessoaModel>> Handle(PessoaObterCommandQuery request, CancellationToken cancellationToken)
    {
        var pessoaId = request?.Id ?? throw new ArgumentNullException(nameof(request.Id));

        try
        {
            var resultadoConsulta = await _obterPessoaQuery.ObterPessoaAsync(pessoaId);
            return resultadoConsulta;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ERROMESSAGE, pessoaId);
            
            return ResultadoConsulta<PessoaModel>.Falha(string.Format(ERROMESSAGE, pessoaId));
        }
    }
}