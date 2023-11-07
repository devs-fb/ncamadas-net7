namespace ModeloSimples.Application.Queries;

using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;
using ModeloSimples.Infrastructure.Shared.Interfaces.Queries;

public class PessoaAuditarCommandQuery : IRequest<ResultadoConsulta<List<PessoaAuditadaModel>>>
{
    public Guid Id { get; }

    public PessoaAuditarCommandQuery(Guid id)
    {
        Id = id;
    }
}

public class PessoaAuditarCommandQueryHandler : IRequestHandler<PessoaAuditarCommandQuery, ResultadoConsulta<List<PessoaAuditadaModel>>>
{
    private const string ERROMESSAGE = "Erro ao auditar pessoa com ID: {0}\n";

    private readonly ILogger<PessoaAuditarCommandQueryHandler> _logger;
    private readonly IPessoaAuditarQuery _auditarPessoaQuery;

    public PessoaAuditarCommandQueryHandler(ILogger<PessoaAuditarCommandQueryHandler> logger, IPessoaAuditarQuery auditarPessoaQuery)
    {
        _logger = logger;
        _auditarPessoaQuery = auditarPessoaQuery ?? throw new ArgumentNullException(nameof(auditarPessoaQuery));
    }

    public async Task<ResultadoConsulta<List<PessoaAuditadaModel>>> Handle(PessoaAuditarCommandQuery request, CancellationToken cancellationToken)
    {
        var pessoaId = request?.Id ?? throw new ArgumentNullException(nameof(request.Id));

        try
        {
            var resultadoConsulta = await _auditarPessoaQuery.AuditarPessoaAsync(pessoaId);
            return resultadoConsulta;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ERROMESSAGE, pessoaId);
            
            return ResultadoConsulta<List<PessoaAuditadaModel>>.Falha(string.Format(ERROMESSAGE, pessoaId));
        }
    }
}