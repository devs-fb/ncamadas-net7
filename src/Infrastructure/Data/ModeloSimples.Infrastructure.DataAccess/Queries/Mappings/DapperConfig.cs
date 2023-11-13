namespace ModeloSimples.Infrastructure.DataAccess.Queries.Mappings;

using Dapper.FluentMap;
using Dapper.FluentMap.Configuration;
using Dapper.FluentMap.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class DapperConfig
{
    private static bool _isConfigured = false;
    public static void ConfigureMappings()
    {
        if (!_isConfigured)
        {
            FluentMapper.EntityMaps.Clear();
            FluentMapper.Initialize(config => {});

            _ = FluentMapper.EntityMaps.TryAdd(typeof(PessoaModelMapping), new PessoaModelMapping());
            _ = FluentMapper.EntityMaps.TryAdd(typeof(PessoaFisicaModelMapping), new PessoaFisicaModelMapping());
            _ = FluentMapper.EntityMaps.TryAdd(typeof(PessoaJuridicaModelMapping), new PessoaJuridicaModelMapping());

            _isConfigured = true;
        }
    }
}
