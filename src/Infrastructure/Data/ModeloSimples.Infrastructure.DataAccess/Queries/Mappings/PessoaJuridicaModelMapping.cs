namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap.Dommel.Mapping;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaJuridicaModelMapping : DommelEntityMap<PessoaJuridicaModel>
{
    public PessoaJuridicaModelMapping()
    {
        ToTable("PessoaJuridica");
        Map(p => p.RazaoSocial).ToColumn("RazaoSocial");
        Map(p => p.NomeFantasia).ToColumn("NomeFantasia");
        Map(p => p.CNAE).ToColumn("CNAE");
    }
}
