namespace ModeloSimples.Infrastructure.Shared.Common;

public class OrdenacaoInfo
{
    public string Campo { get; set; }
    public bool Ascendente { get; set; }

    public OrdenacaoInfo(string campo, bool ascendente)
    {
        Campo = campo;
        Ascendente = ascendente;
    }
}
