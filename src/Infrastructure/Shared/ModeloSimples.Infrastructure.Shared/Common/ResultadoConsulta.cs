namespace ModeloSimples.Infrastructure.Shared.Common;

public class ResultadoConsulta<T>
{
    public bool ComSucesso { get; private set; }

    public string TipoDeDados { get; private set; }
    public T Dados { get; private set; }
    public string MensagemErro { get; private set; }
    public DataInfo PaginacaoOrdenacao { get; private set; }

    private ResultadoConsulta(bool sucesso, string tipoDeDados, T dados, DataInfo paginacaoOrdenacao, string mensagemErro)
    {
        ComSucesso = sucesso;
        TipoDeDados = tipoDeDados;
        Dados = dados;
        PaginacaoOrdenacao = paginacaoOrdenacao;
        MensagemErro = mensagemErro;
    }

    public static ResultadoConsulta<T> Sucesso(T dados, DataInfo paginacaoOrdenacao)
    {
        var tipoDeDados = typeof(T).Name;
        return new ResultadoConsulta<T>(true, tipoDeDados, dados, paginacaoOrdenacao, null);
    }

    public static ResultadoConsulta<T> Falha(string mensagemErro)
    {
        var tipoDeDados = typeof(T).Name;
        return new ResultadoConsulta<T>(false, tipoDeDados, default, null, mensagemErro);
    }
}
