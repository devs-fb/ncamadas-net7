namespace ModeloSimples.Infrastructure.Shared.Common;

public class ResultadoOperacao<T>
{
    public bool ComSucesso { get; private set; }
    public string TipoDeDados { get; private set; }
    public T Dados { get; private set; }
    public string MensagemErro { get; private set; }

    private ResultadoOperacao(bool sucesso, string tipoDeDaos, T dados, string mensagemErro)
    {
        ComSucesso = sucesso;
        TipoDeDados = tipoDeDaos;
        Dados = dados;
        MensagemErro = mensagemErro;
    }

    public static ResultadoOperacao<T> Sucesso(T dados)
    {
        var tipoDeDados = typeof(T).Name;
        return new ResultadoOperacao<T>(true, tipoDeDados, dados, null);
    }

    public static ResultadoOperacao<T> Falha(string mensagemErro)
    {
        var tipoDeDados = typeof(T).Name;
        return new ResultadoOperacao<T>(false, tipoDeDados,  default, mensagemErro);
    }
}
