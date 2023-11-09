namespace ModeloSimples.Service.API.Principal.Common;

using System.Collections;

public class Resposta<T>
{
    private const string OperacaoBemSucedida = "Operação bem-sucedida.";
    private const string Desconhecido = "Desconhecido";

    public bool Sucesso { get; private set; }
    public string TipoDeDados { get; private set; }
    public T Dados { get; private set; }
    public string Mensagem { get; private set; }

    public Resposta(T dados)
    {
        Sucesso = true;
        TipoDeDados = ObterNomeDoTipo(dados);
        Dados = dados;
        Mensagem = OperacaoBemSucedida;
    }

    public Resposta(string mensagemErro)
    {
        Sucesso = false;
        TipoDeDados = typeof(T).Name;
        Dados = default;
        Mensagem = mensagemErro;
    }

    private static (IEnumerable Enumerable, object Objeto, bool Result) IsEnumerable(object obj) =>
        obj is IEnumerable enumerable && enumerable.GetEnumerator().MoveNext() 
        ? new (enumerable, obj, true) 
        : new (null, obj, false);

    private static string ObterNomeDoTipo(object obj)
    {
        if (obj is not null)
        { 
            var objeto = IsEnumerable(obj);
            if (objeto.Result)
            {
                var tipoDoElemento = objeto.Enumerable.Cast<object>().First().GetType();
                return tipoDoElemento.Name;
            }

            Type tipo = obj.GetType();
            return tipo.Name;
        }
    
        return Desconhecido;
    }
}
