namespace ModeloSimples.Domain.Interfaces;

using ModeloSimples.Domain.Aggregates;

public interface IPessoaService
{
    Task<Pessoa> CriarPessoa(Pessoa pessoa, CancellationToken cancellationToken);
    Task<Pessoa> EditarPessoa(Guid pessoaId, Pessoa pessoa);
    Task<bool> RemoverPessoa(Guid pessoaId);
    Task<bool> BloquearPessoa(Guid pessoaId);
    Task<bool> DesbloquearPessoa(Guid pessoaId);
}
