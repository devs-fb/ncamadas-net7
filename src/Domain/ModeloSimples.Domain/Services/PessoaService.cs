namespace ModeloSimples.Domain.Services;

using Microsoft.Extensions.Logging;
using ModeloSimples.Domain.Aggregates;
using ModeloSimples.Domain.Entities;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Interfaces;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaService : IPessoaService
{
    private const string ERRORMESSAGE = "Um erro ocorreu ao tentar salvar";
    private const string SAVEFAIL = "Não foi possível salvar ID {0}";
    private const string NOTFOUND = "Pessoa não encontrada. ID {0}";
    private const string BLOCKED = "Pessoa bloqueada para mudanças. ID {0}";

    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PessoaService> _logger;
    private readonly IRepository<Pessoa> _pessoaRepository;
    private readonly IRepository<PessoaFisica> _pessoaFisicaRepository;
    private readonly IRepository<PessoaJuridica> _pessoaJuridicaRepository;

    public PessoaService(IUnitOfWork unitOfWork, ILogger<PessoaService> logger, IRepository<Pessoa> pessoaRepository, IRepository<PessoaFisica> pessoaFisicaRepository, IRepository<PessoaJuridica> pessoaJuridicaRepository)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _pessoaRepository = pessoaRepository ?? throw new ArgumentNullException(nameof(pessoaRepository));
        _pessoaFisicaRepository = pessoaFisicaRepository ?? throw new ArgumentNullException(nameof(pessoaFisicaRepository));
        _pessoaJuridicaRepository = pessoaJuridicaRepository ?? throw new ArgumentNullException(nameof(pessoaJuridicaRepository));
    }

    public async Task<Pessoa> CriarPessoa(Pessoa pessoa, CancellationToken cancellationToken)
    {
        await DefinirIdentificador(pessoa, cancellationToken);

        _unitOfWork.BeginTransaction();

        try
        {
            pessoa.LimparEventos();

            if (pessoa.CriarNovaPessoa())
            {
                await _pessoaRepository.AddAsync(pessoa, cancellationToken);
                var result = await _unitOfWork.CommitAsync();

                if (result.Equals(0))
                {
                    _logger.LogWarning(message: SAVEFAIL, pessoa.PessoaId);
                    _unitOfWork.Rollback();

                    return null;
                }

                return pessoa;
            }

            _unitOfWork.Rollback();

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ERRORMESSAGE);
            _unitOfWork.Rollback();

            return null;
        }
    }

    public async Task<Pessoa> EditarPessoa(Guid pessoaId, Pessoa pessoa)
    {
        var pessoaExistente = await Obter(pessoaId);

        _unitOfWork.BeginTransaction();

        try
        {
            pessoaExistente.LimparEventos();
            pessoaExistente.EditarPessoa(pessoa);

            _pessoaRepository.Update(pessoaExistente);

            var result = await _unitOfWork.CommitAsync();

            if (result.Equals(0))
            {
                _logger.LogWarning(message: SAVEFAIL, pessoaId);
                _unitOfWork.Rollback();

                return null;
            }

            return pessoaExistente;
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, pessoaId));
            throw ex;
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, pessoaId));
            throw ex;
        }
        catch 
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<bool> RemoverPessoa(Guid pessoaId)
    {
        var pessoaExistente = await Obter(pessoaId);

        _unitOfWork.BeginTransaction();

        try
        {
            pessoaExistente.LimparEventos();

            if (pessoaExistente.RemoverPessoa())
            {
                _pessoaRepository.Update(pessoaExistente);

                var result = await _unitOfWork.CommitAsync();

                if (result.Equals(0))
                {
                    _logger.LogWarning(message: SAVEFAIL, pessoaId);
                    _unitOfWork.Rollback();
                    
                    return false;
                }

                return true;
            }
            
            return false;
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, pessoaId));
            throw ex;
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, pessoaId));
            throw ex;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<bool> BloquearPessoa(Guid pessoaId)
    {
        var pessoaExistente = await Obter(pessoaId);

        _unitOfWork.BeginTransaction();

        try
        {
            pessoaExistente.LimparEventos();

            if (pessoaExistente.BloquearPessoa())
            {
                _pessoaRepository.Update(pessoaExistente);

                var result = await _unitOfWork.CommitAsync();

                if (result.Equals(0))
                {
                    _logger.LogWarning(message: SAVEFAIL, pessoaId);
                    _unitOfWork.Rollback();

                    return false;
                }

                return true;
            }

            return false;
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, pessoaId));
            throw ex;
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, pessoaId));
            throw ex;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }

    public async Task<bool> DesbloquearPessoa(Guid pessoaId)
    {
        var pessoaExistente = await Obter(pessoaId, validarBloqueio: false);

        _unitOfWork.BeginTransaction();

        try
        {
            pessoaExistente.LimparEventos();

            if (pessoaExistente.DesbloquearPessoa())
            {
                _pessoaRepository.Update(pessoaExistente);

                var result = await _unitOfWork.CommitAsync();

                if (result.Equals(0))
                {
                    _logger.LogWarning(message: SAVEFAIL, pessoaId);
                    _unitOfWork.Rollback();

                    return false;
                }

                return true;
            }

            return false;
        }
        catch (InexistenteException ex)
        {
            _logger.LogError(ex, string.Format(NOTFOUND, pessoaId));
            throw ex;
        }
        catch (BloqueioException ex)
        {
            _logger.LogError(ex, string.Format(BLOCKED, pessoaId));
            throw ex;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }


    private async Task<Guid> GerarNovoIdUnico(CancellationToken cancellationToken)
    {
        Guid novoId;

        do { novoId = Guid.NewGuid(); }
        while (await ExistePessoa(novoId, cancellationToken));

        return novoId;
    }

    private async Task<bool> ExistePessoa(Guid id, CancellationToken cancellationToken)
        => (await _pessoaRepository.GetByIdAsync(id, cancellationToken) is not null);

    private async Task DefinirIdentificador(Pessoa pessoa, CancellationToken cancellationToken)
    {
        pessoa.PessoaId = (pessoa.PessoaId.Equals(Guid.Empty) || (await ExistePessoa(pessoa.PessoaId, cancellationToken)))
            ? await GerarNovoIdUnico(cancellationToken)
            : pessoa.PessoaId;
    }

    private async Task<Pessoa> Obter(Guid pessoaId, bool validarBloqueio = true)
    {
        var pessoaExistente = await _pessoaRepository.GetByIdAsync(pessoaId);

        Validar(pessoaId, pessoaExistente, validarBloqueio);

        pessoaExistente.PessoaFisica = await _pessoaFisicaRepository.GetByIdAsync(pessoaId);
        pessoaExistente.PessoaJuridica = await _pessoaJuridicaRepository.GetByIdAsync(pessoaId);

        return pessoaExistente;
    }

    private void Validar(Guid pessoaId, Pessoa pessoaExistente, bool validarBloqueio)
    {
        ValidarEntrada(pessoaId, pessoaExistente);
        ValidarControle(pessoaId, pessoaExistente, validarBloqueio);
    }

    private void ValidarEntrada(Guid pessoaId, Pessoa pessoaExistente)
    {
        if (pessoaExistente is null)
        {
            _logger.LogWarning(message: NOTFOUND, pessoaId);

            throw new InexistenteException(string.Format(NOTFOUND, pessoaId));
        }
    }

    private void ValidarControle(Guid pessoaId, Pessoa pessoaExistente, bool validarBloqueio)
    {
        if (validarBloqueio && pessoaExistente.Controle.IsBlocked)
        {
            _logger.LogWarning(message: BLOCKED, pessoaId);

            throw new BloqueioException(string.Format(BLOCKED, pessoaId));
        }
    }
}
