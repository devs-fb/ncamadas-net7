namespace ModeloSimples.Infrastructure.Shared.Common;

public class PaginacaoInfo
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalRegistro { get;  set; }

    public PaginacaoInfo(int pagina, int tamanhoPagina, int totalRegistro)
    {
        Pagina = pagina;
        TamanhoPagina = tamanhoPagina;
        TotalRegistro = totalRegistro;
    }
}
