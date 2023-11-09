namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap.Dommel.Mapping;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaFisicaModelMapping : DommelEntityMap<PessoaFisicaModel>
{
    public PessoaFisicaModelMapping()
    {
        ToTable(DapperConstant.PessoaFisicaModel.PessoaFisica);
        Map(p => p.NomeSocial).ToColumn(DapperConstant.PessoaFisicaModel.NomeSocial);
        Map(p => p.DataNascimento).ToColumn(DapperConstant.PessoaFisicaModel.DataNascimento);
        Map(p => p.Genero).ToColumn(DapperConstant.PessoaFisicaModel.Genero);
    }
}
