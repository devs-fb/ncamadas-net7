namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap;

public class DapperConfig
{
    public static void ConfigureMappings()
    {
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new PessoaModelMapping());
            config.AddMap(new PessoaFisicaModelMapping());
            config.AddMap(new PessoaJuridicaModelMapping());
        });
    }
}
