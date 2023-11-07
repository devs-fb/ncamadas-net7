namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap.Dommel.Mapping;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaFisicaModelMapping : DommelEntityMap<PessoaFisicaModel>
{
    public PessoaFisicaModelMapping()
    {
        ToTable("PessoaFisica");
        Map(p => p.NomeSocial).ToColumn("NomeSocial");
        Map(p => p.DataNascimento).ToColumn("DataNascimento");
        Map(p => p.Genero).ToColumn("Genero");
    }
}
