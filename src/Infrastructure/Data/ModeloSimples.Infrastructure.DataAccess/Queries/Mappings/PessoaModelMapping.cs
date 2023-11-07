namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap.Dommel.Mapping;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaModelMapping : DommelEntityMap<PessoaModel>
{
    public PessoaModelMapping()
    {
        ToTable("Pessoa");
        Map(p => p.Id).ToColumn("PessoaId").IsKey();
        Map(p => p.Tipo).ToColumn("Tipo");   
    }
}
