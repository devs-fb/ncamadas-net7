namespace ModeloSimples.Application.Mappings;

using AutoMapper;
using ModeloSimples.Domain.Aggregates;
using ModeloSimples.Infrastructure.Shared.DTO;

public class PessoaProfile : Profile
{
    public PessoaProfile()
    {
        CreateMap<PessoaModel, Pessoa>()
            .ForMember(dest => dest.PessoaId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo == "F" ? "PessoaFisica" : src.Tipo == "J" ? "PessoaJuridica" : "Outros"))
            .ForPath(dest => dest.PessoaJuridica.PessoaJuridicaId, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaJuridica.RazaoSocial, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaJuridica.NomeFantasia, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaJuridica.CNAE, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.PessoaFisicaId, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.NomeSocial, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.Genero, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.DataNascimento, opt => opt.Ignore())
            .BeforeMap((src, dest) =>
            {
                dest.PessoaFisica = null;
                dest.PessoaJuridica = null;

                switch (src.Tipo.Trim().ToUpperInvariant())
                {
                    case "F":
                        dest.AssociarPessoaFisica(new(
                            pessoaFisicaId: src.Id,
                            nomeSocial: src.PessoaFisica.NomeSocial,
                            dataNascimento: src.PessoaFisica.DataNascimento,
                            genero: src.PessoaFisica.Genero));
                        break;
                    case "J":
                        dest.AssociarPessoaJuridica(new(
                            pessoaJuridicaId: src.Id,
                            razaoSocial: src.PessoaJuridica.RazaoSocial,
                            nomeFantasia: src.PessoaJuridica.NomeFantasia,
                            cnae: src.PessoaJuridica.CNAE));
                        break;
                }
            });

        CreateMap<Pessoa, PessoaModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PessoaId))
            .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo == "PessoaFisica" ? "F" : src.Tipo == "PessoaJuridica" ? "Outros" : ""))
            .ForPath(dest => dest.PessoaJuridica.RazaoSocial, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaJuridica.NomeFantasia, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaJuridica.CNAE, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.NomeSocial, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.Genero, opt => opt.Ignore())
            .ForPath(dest => dest.PessoaFisica.DataNascimento, opt => opt.Ignore())
            .BeforeMap((src, dest) =>
            {
                dest.PessoaFisica = null;
                dest.PessoaJuridica = null;

                switch (src.Tipo.Trim().ToUpperInvariant())
                {
                    case "PessoaFisica":
                        dest.PessoaFisica = new()
                        {
                            NomeSocial = src.PessoaFisica.NomeSocial,
                            DataNascimento = src.PessoaFisica.DataNascimento,
                            Genero = src.PessoaFisica.Genero
                        };
                        break;
                    case "PessoaJuridica":
                        dest.PessoaJuridica = new()
                        {
                            RazaoSocial = src.PessoaJuridica.RazaoSocial,
                            NomeFantasia = src.PessoaJuridica.NomeFantasia,
                            CNAE = src.PessoaJuridica.CNAE
                        };
                        break;
                }
            });
    }
}
