namespace ModeloSimples.Domain.Exceptions;

public class InexistenteException : Exception
{
    public InexistenteException(string mensagem) : base(mensagem)
    {
    }
}