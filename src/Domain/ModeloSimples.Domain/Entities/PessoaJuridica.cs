namespace ModeloSimples.Domain.Entities;

using ModeloSimples.Domain.Base;
using ModeloSimples.Domain.Events;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Validators;
using System.ComponentModel.DataAnnotations;

public class PessoaJuridica : Entity
{
    private const string Line = "\n";

    private PessoaJuridica() { }

    [Key]
    [Required]
    public Guid PessoaJuridicaId { get; set; }

    public string RazaoSocial { get; private set; }
    public string NomeFantasia { get; private set; }
    public string CNAE { get; private set; }

    public PessoaJuridica(Guid pessoaJuridicaId, string razaoSocial, string nomeFantasia, string cnae)
    {
        PessoaJuridicaId = pessoaJuridicaId;
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        CNAE = cnae;
    }

    public bool CriarNovaPessoaJuridica()
    {
        var roles = new PessoaJuridicaValidator();

        var resultRoles = roles.Validate(this);

        if (!resultRoles.IsValid)
        {
            throw new ValidacaoException(string.Join(Line, resultRoles.Errors.Select(e => e.ErrorMessage)));
        }

        AddDomainEvent(new PessoaJuridicaCriadaEvent(PessoaJuridicaId));

        return true;
    }

    public bool EditarPessoaJuridica(PessoaJuridica pessoaJuridica)
    {
        RazaoSocial = pessoaJuridica.RazaoSocial;
        NomeFantasia = pessoaJuridica.NomeFantasia;
        CNAE = pessoaJuridica.CNAE;

        var roles = new PessoaJuridicaValidator();

        var resultRoles = roles.Validate(this);

        if (!resultRoles.IsValid)
        {
            throw new ValidacaoException(string.Join(Line, resultRoles.Errors.Select(e => e.ErrorMessage)));
        }

        AddDomainEvent(new PessoaJuridicaEditadaEvent(PessoaJuridicaId));

        return true;
    }

    public bool RemoverPessoaJuridica()
    {
        AddDomainEvent(new PessoaJuridicaRemovidaEvent(PessoaJuridicaId));

        return true;
    }
}
