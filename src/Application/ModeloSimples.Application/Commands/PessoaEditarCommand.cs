namespace ModeloSimples.Application.Commands;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Aggregates;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaEditarCommand : IRequest<ResultadoOperacao<PessoaModel>>
{
    public Guid PessoaId { get; }
    public PessoaModel Pessoa { get; }

    public PessoaEditarCommand(Guid pessoaId, PessoaModel pessoa)
    {
        PessoaId = pessoaId;
        Pessoa = pessoa;
    }
}

public class PessoaEditarCommandHandler : IRequestHandler<PessoaEditarCommand, ResultadoOperacao<PessoaModel>>
{
    private const string SUCCESSMESSAGE = "Edição realizada: ID {0}";
    private const string INVALIDINFO = "Informações inválidas";
    private const string SAVEFAIL = "Não foi possível salvar ID {0}";
    private const string ERRORMESSAGE = "Um erro ocorreu ao tentar salvar";
    private const string NOTFOUND = "Pessoa não encontrada. ID {0}";
    private const string BLOCKED = "Pessoa bloqueada para mudanças. ID {0}";

    private readonly IMapper _mapper;
    private readonly ILogger<PessoaEditarCommandHandler> _logger;
    private readonly IPessoaService _pessoaService;
    
    public PessoaEditarCommandHandler(IMapper mapper, ILogger<PessoaEditarCommandHandler> logger, IPessoaService pessoaService)
    {
        _mapper = mapper;
        _logger = logger;
        _pessoaService = pessoaService;
    }

    public async Task<ResultadoOperacao<PessoaModel>> Handle(PessoaEditarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Validar(request);

            var pessoa = _mapper.Map<Pessoa>(request.Pessoa);
            
            var pessoaEditada = await _pessoaService.EditarPessoa(request.PessoaId, pessoa);

            if (pessoaEditada is null)
            {
                return ResultadoOperacao<PessoaModel>.Falha(string.Format(SAVEFAIL, request.PessoaId));
            }

            var pessoaModel = _mapper.Map<PessoaModel>(pessoaEditada);

            _logger.LogInformation(message: SUCCESSMESSAGE, pessoaModel.Id);

            return ResultadoOperacao<PessoaModel>.Sucesso(pessoaModel);
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, request.PessoaId));
            
            return ResultadoOperacao<PessoaModel>.Falha(ex.Message);
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, request.PessoaId));
            
            return ResultadoOperacao<PessoaModel>.Falha(ex.Message);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);

            return ResultadoOperacao<PessoaModel>.Falha(INVALIDINFO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);

            return ResultadoOperacao<PessoaModel>.Falha(ex.Message);
        }
    }

    private void Validar(PessoaEditarCommand request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (request.PessoaId.Equals(Guid.Empty)) throw new ArgumentNullException(nameof(request.PessoaId));
    }
}