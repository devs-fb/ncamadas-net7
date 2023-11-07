namespace ModeloSimples.Application.Commands;

using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Infrastructure.Shared.Common;

public class PessoaRemoverCommand : IRequest<ResultadoOperacao<Unit>>
{
    public Guid PessoaId { get; }

    public PessoaRemoverCommand(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }
}

public class PessoaRemoverCommandHandler : IRequestHandler<PessoaRemoverCommand, ResultadoOperacao<Unit>>
{
    private const string SUCCESSMESSAGE = "Remoção realizada: ID {0}";
    private const string INVALIDINFO = "Informações inválidas";
    private const string REMOVEFAIL = "Não foi possível remover";
    private const string ERRORMESSAGE = "Um erro ocorreu ao tentar remover";
    private const string NOTFOUND = "Pessoa não encontrada. ID {0}";
    private const string BLOCKED = "Pessoa bloqueada para mudanças. ID {0}";

    private readonly ILogger<PessoaRemoverCommandHandler> _logger;
    private readonly IPessoaService _pessoaService;

    public PessoaRemoverCommandHandler(ILogger<PessoaRemoverCommandHandler> logger, IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
        _logger = logger;
    }

    public async Task<ResultadoOperacao<Unit>> Handle(PessoaRemoverCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Validar(request);

            var pessoaRemovida = await _pessoaService.RemoverPessoa(request.PessoaId);

            if (!pessoaRemovida)
            {
                return ResultadoOperacao<Unit>.Falha(REMOVEFAIL);
            }

            _logger.LogInformation(message: SUCCESSMESSAGE, request.PessoaId);

            return ResultadoOperacao<Unit>.Sucesso(Unit.Value);
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, request.PessoaId));

            return ResultadoOperacao<Unit>.Falha(ex.Message);
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, request.PessoaId));

            return ResultadoOperacao<Unit>.Falha(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);

            return ResultadoOperacao<Unit>.Falha(INVALIDINFO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);

            return ResultadoOperacao<Unit>.Falha(ex.Message);
        }
    }

    private void Validar(PessoaRemoverCommand request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.PessoaId.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(request.PessoaId));
    }
}