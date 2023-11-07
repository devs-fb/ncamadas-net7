namespace ModeloSimples.Application.Commands;

using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Infrastructure.Shared.Common;

public class PessoaBloquearCommand : IRequest<ResultadoOperacao<bool>>
{
    public PessoaBloquearCommand (Guid pessoaId)
    {
        PessoaId = pessoaId;
    }

    public Guid PessoaId { get; private set; }
}

public class PessoaBloquearCommandHandler : IRequestHandler<PessoaBloquearCommand, ResultadoOperacao<bool>>
{
    private const string SUCCESSMESSAGE = "Bloqueio realizado: ID {0}";
    private const string INVALIDINFO = "Informações inválidas";
    private const string REMOVEFAIL = "Não foi possível bloquear";
    private const string ERRORMESSAGE = "Um erro ocorreu ao tentar bloquear";
    private const string NOTFOUND = "Pessoa não encontrada. ID {0}";
    private const string BLOCKED = "Pessoa bloqueada para mudanças. ID {0}";

    private readonly ILogger<PessoaBloquearCommandHandler> _logger;
    private readonly IPessoaService _pessoaService;

    public PessoaBloquearCommandHandler(ILogger<PessoaBloquearCommandHandler> logger, IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
        _logger = logger;
    }

    public async Task<ResultadoOperacao<bool>> Handle(PessoaBloquearCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Validar(request);

            var pessoabloqueada = await _pessoaService.BloquearPessoa(request.PessoaId);

            if (!pessoabloqueada)
            {
                return ResultadoOperacao<bool>.Falha(REMOVEFAIL);
            }

            _logger.LogInformation(message: SUCCESSMESSAGE, request.PessoaId);

            return ResultadoOperacao<bool>.Sucesso(true);
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, request.PessoaId));

            return ResultadoOperacao<bool>.Falha(ex.Message);
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, request.PessoaId));

            return ResultadoOperacao<bool>.Falha(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);

            return ResultadoOperacao<bool>.Falha(INVALIDINFO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);

            return ResultadoOperacao<bool>.Falha(ex.Message);
        }
    }

    private void Validar(PessoaBloquearCommand request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.PessoaId.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(request.PessoaId));
    }
}
