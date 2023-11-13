namespace ModeloSimples.Infrastructure.DataAccess.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ModeloSimples.Domain.Aggregates;
using ModeloSimples.Domain.Entities;
using Newtonsoft.Json;

public class PessoaMapping
{
    public PessoaMapping(EntityTypeBuilder<Pessoa> builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Property(p=> p.Tipo).HasMaxLength(50);

        builder.OwnsOne(p => p.Controle, auditoria =>
        {
            auditoria.Property(a => a.Created).HasColumnName(MappingConstant.Auditoria.Criado);
            auditoria.Property(a => a.Modified).HasColumnName(MappingConstant.Auditoria.Modificado);
            auditoria.Property(a => a.IsRemoved).HasColumnName(MappingConstant.Auditoria.Removido);
            auditoria.Property(a => a.IsBlocked).HasColumnName(MappingConstant.Auditoria.Bloquado);
        });

        builder.OwnsOne(p => p.Versao, versao =>
        {
            versao.Property<IReadOnlyCollection<Pessoa>>(MappingConstant.Versionamento.Versao)
                .HasColumnName(MappingConstant.Versionamento.Versao)
                .HasColumnType(MappingConstant.ColumnType.NVarCharMax)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, Formatting.None),
                    v => JsonConvert.DeserializeObject<IReadOnlyCollection<Pessoa>>(v)
                )
                .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<Pessoa>>(
                    (c1, c2) => JsonConvert.SerializeObject(c1) == JsonConvert.SerializeObject(c2),
                    c => c == null ? 0 : JsonConvert.SerializeObject(c).GetHashCode(),
                    c => JsonConvert.DeserializeObject<IReadOnlyCollection<Pessoa>>(JsonConvert.SerializeObject(c))
                ));
        });

        builder.HasOne(p => p.PessoaFisica)
            .WithOne()
            .HasForeignKey<PessoaFisica>(pf => pf.PessoaFisicaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.PessoaJuridica)
            .WithOne()
            .HasForeignKey<PessoaJuridica>(pf => pf.PessoaJuridicaId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
