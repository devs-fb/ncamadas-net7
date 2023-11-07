namespace ModeloSimples.Infrastructure.Shared.Interfaces;

public interface ICommandFactory<TCommand, TEntidade>
{
    TCommand CriarComando(TEntidade entidade);
}
