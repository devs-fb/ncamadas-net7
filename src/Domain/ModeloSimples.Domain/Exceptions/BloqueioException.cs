namespace ModeloSimples.Domain.Exceptions;

public class BloqueioException : Exception
{
    public BloqueioException(string mensagem) : base(mensagem)
    {
    }
}