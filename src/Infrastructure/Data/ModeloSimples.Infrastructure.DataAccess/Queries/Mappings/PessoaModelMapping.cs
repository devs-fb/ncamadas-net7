namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap.Dommel.Mapping;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaModelMapping : DommelEntityMap<PessoaModel>
{
    public PessoaModelMapping()
    {
        ToTable(DapperConstant.PessoaModel.Pessoa);
        Map(p => p.Id).ToColumn(DapperConstant.PessoaModel.PessoaId).IsKey();
        Map(p => p.Tipo).ToColumn(DapperConstant.PessoaModel.Tipo);   
    }
}
