namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap.Dommel.Mapping;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaJuridicaModelMapping : DommelEntityMap<PessoaJuridicaModel>
{
    public PessoaJuridicaModelMapping()
    {
        ToTable(DapperConstant.PessoaJuridicaModel.PessoaJuridica);
        Map(p => p.RazaoSocial).ToColumn(DapperConstant.PessoaJuridicaModel.RazaoSocial);
        Map(p => p.NomeFantasia).ToColumn(DapperConstant.PessoaJuridicaModel.NomeFantasia);
        Map(p => p.CNAE).ToColumn(DapperConstant.PessoaJuridicaModel.CNAE);
    }
}
