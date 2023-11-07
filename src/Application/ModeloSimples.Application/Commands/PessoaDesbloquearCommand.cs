namespace ModeloSimples.Application.Commands;

using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Infrastructure.Shared.Common;

public class PessoaDesbloquearCommand : IRequest<ResultadoOperacao<bool>>
{
    public PessoaDesbloquearCommand(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }

    public Guid PessoaId { get; private set; }
}

public class PessoaDesbloquearCommandHandler : IRequestHandler<PessoaDesbloquearCommand, ResultadoOperacao<bool>>
{
    private const string SUCCESSMESSAGE = "Bloqueio realizado: ID {0}";
    private const string INVALIDINFO = "Informações inválidas";
    private const string REMOVEFAIL = "Não foi possível Desbloquear";
    private const string ERRORMESSAGE = "Um erro ocorreu ao tentar Desbloquear";
    private const string NOTFOUND = "Pessoa não encontrada. ID {0}";
    private const string BLOCKED = "Pessoa bloqueada para mudanças. ID {0}";

    private readonly ILogger<PessoaDesbloquearCommandHandler> _logger;
    private readonly IPessoaService _pessoaService;

    public PessoaDesbloquearCommandHandler(ILogger<PessoaDesbloquearCommandHandler> logger, IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
        _logger = logger;
    }

    public async Task<ResultadoOperacao<bool>> Handle(PessoaDesbloquearCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Validar(request);

            var pessoabloqueada = await _pessoaService.DesbloquearPessoa(request.PessoaId);

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

    private void Validar(PessoaDesbloquearCommand request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.PessoaId.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(request.PessoaId));
    }
}