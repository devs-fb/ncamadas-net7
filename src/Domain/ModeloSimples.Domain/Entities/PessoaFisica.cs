namespace ModeloSimples.Domain.Entities;

using ModeloSimples.Domain.Base;
using ModeloSimples.Domain.Events;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Validators;
using System.ComponentModel.DataAnnotations;

public class PessoaFisica : Entity
{
    private const string Line = "\n";

    private PessoaFisica() { }

    [Key]
    [Required]
    public Guid PessoaFisicaId { get; set; }

    public string NomeSocial { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public string Genero { get; private set; }

    public PessoaFisica(Guid pessoaFisicaId, string nomeSocial, DateTime dataNascimento, string genero)
    {
        PessoaFisicaId = pessoaFisicaId;
        NomeSocial = nomeSocial;
        DataNascimento = dataNascimento;
        Genero = genero;
    }

    public bool CriarNovaPessoaFisica()
    {
        var roles = new PessoaFisicaValidator();

        var resultRoles = roles.Validate(this);
        
        if (!resultRoles.IsValid) 
        {
            throw new ValidacaoException(string.Join(Line, resultRoles.Errors.Select(e => e.ErrorMessage)));
        }

        AddDomainEvent(new PessoaFisicaCriadaEvent(PessoaFisicaId));

        return true;
    }

    public bool EditarPessoaFisica(PessoaFisica pessoaFisica)
    {
        NomeSocial = pessoaFisica.NomeSocial;
        DataNascimento = pessoaFisica.DataNascimento;
        Genero = pessoaFisica.Genero;

        var roles = new PessoaFisicaValidator();

        var resultRoles = roles.Validate(this);

        if (!resultRoles.IsValid)
        {
            throw new ValidacaoException(string.Join(Line, resultRoles.Errors.Select(e => e.ErrorMessage)));
        }

        AddDomainEvent(new PessoaFisicaEditadaEvent(PessoaFisicaId));

        return true;
    }

    public bool RemoverPessoaFisica()
    {
        AddDomainEvent(new PessoaFisicaRemovidaEvent(PessoaFisicaId));

        return true;
    }
}
