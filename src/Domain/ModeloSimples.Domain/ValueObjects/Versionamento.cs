namespace ModeloSimples.Domain.ValueObjects;

using ModeloSimples.Domain.Aggregates;
using Newtonsoft.Json;

public sealed class Versionamento 
{
    private Versionamento() { }
    public Versionamento(IReadOnlyCollection<Pessoa> dados)
    {
        Dados = dados ?? throw new ArgumentNullException(nameof(dados));
    }

    [JsonIgnore]
    public IReadOnlyCollection<Pessoa> Dados { get; }
}

