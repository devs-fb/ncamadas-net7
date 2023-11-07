namespace ModeloSimples.Application.Commands;

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Aggregates;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Infrastructure.Shared.Common;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaCriarCommand : IRequest<ResultadoOperacao<PessoaModel>>
{
    public PessoaModel Pessoa { get; }

    public PessoaCriarCommand(PessoaModel pessoa)
    {
        Pessoa = pessoa;
    }
}

public class PessoaCriarCommandHandler : IRequestHandler<PessoaCriarCommand, ResultadoOperacao<PessoaModel>>
{
    private const string SUCCESSMESSAGE = "Criação realizada: ID {0}";
    private const string INVALIDINFO = "Informações inválidas";
    private const string MAPPERORDOMAINFAIL = "Falha ao tentar mapear a entidade ou definir o domínio";
    private const string ERRORMESSAGE = "Um erro ocorreu ao tentar salvar";

    private readonly ILogger<PessoaCriarCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IPessoaService _pessoaService;

    public PessoaCriarCommandHandler(ILogger<PessoaCriarCommandHandler> logger, IMapper mapper, IPessoaService pessoaService)
    {
        _logger = logger;
        _mapper = mapper;
        _pessoaService = pessoaService;
    }

    public async Task<ResultadoOperacao<PessoaModel>> Handle(PessoaCriarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Validar(request);

            var pessoa = _mapper.Map<PessoaModel, Pessoa>(request.Pessoa);

            var novaPessoa = await _pessoaService.CriarPessoa(pessoa, cancellationToken);

            if (novaPessoa is null)
            {
                return ResultadoOperacao<PessoaModel>.Falha(MAPPERORDOMAINFAIL);
            }

            _logger.LogInformation(message: SUCCESSMESSAGE, novaPessoa.PessoaId);

            var pessoaModel = _mapper.Map<Pessoa, PessoaModel>(novaPessoa);

            return ResultadoOperacao<PessoaModel>.Sucesso(pessoaModel);
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

    private void Validar(PessoaCriarCommand request)
    {
        if (request.Pessoa is null) throw new ArgumentNullException(nameof(request.Pessoa));
    }
}