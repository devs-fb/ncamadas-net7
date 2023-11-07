namespace ModeloSimples.Domain.Exceptions;

public class ValidacaoException : Exception
{
    public ValidacaoException(string mensagem) : base(mensagem)
    {
    }
}