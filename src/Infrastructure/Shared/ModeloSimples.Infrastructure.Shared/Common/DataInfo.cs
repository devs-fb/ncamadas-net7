namespace ModeloSimples.Infrastructure.Shared.Common;

public class DataInfo
{
    public PaginacaoInfo Paginacao { get; set; }
    public IList<OrdenacaoInfo> Ordenacao { get; set; }

    public DataInfo(PaginacaoInfo paginacao, IList<OrdenacaoInfo> ordenacao)
    {
        Paginacao = paginacao;
        Ordenacao = ordenacao;
    }
}