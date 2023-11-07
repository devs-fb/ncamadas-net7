namespace ModeloSimples.Domain.Aggregates;

using ModeloSimples.Domain.Base;
using ModeloSimples.Domain.Common.Tools.Converters;
using ModeloSimples.Domain.Entities;
using ModeloSimples.Domain.Events;
using ModeloSimples.Domain.Exceptions;
using ModeloSimples.Domain.Validators;
using ModeloSimples.Domain.ValueObjects;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

public class Pessoa : AggregateRoot
{
    private const string TipoPessoaFisica = "PessoaFisica";
    private const string TipoPessoaJuridica = "PessoaJuridica";

    private const string PessoaNaoPodeSerNula = "A pessoa não pode ser nula.";
    private const string PessoaFisicaNaoPodeSerNula = "A pessoa física não pode ser nula.";
    private const string PessoaJuridicaNaoPodeSerNula = "A pessoa jurídica não pode ser nula.";
    private const string PessoaFisicaNaoAssociada = "A pessoa não está corretamente associada à uma pessoa física.";
    private const string PessoaJuridicaNaoAssociada = "A pessoa não está corretamente associada à uma pessoa jurídica.";

    [Key]
    [Required]
    public Guid PessoaId { get; set; }

    [Required]
    public string Tipo { get; set; }

    [Required]
    public Auditoria Controle { get; set; }

    [Required]
    [JsonConverter(typeof(JsonVersionConverter))]
    [JsonIgnore]
    public Versionamento Versao { get; set; }

    public virtual PessoaFisica PessoaFisica { get; set; }

    public virtual PessoaJuridica PessoaJuridica { get; set; }


    public Pessoa(Guid pessoaId, string tipo)
    {
        PessoaId = pessoaId;
        Tipo = tipo;
    }

    private Pessoa()
    {
        Controle = new(created: DateTime.UtcNow, modified: null, isRemoved: false, isBlocked: false);
        Versao = new(new List<Pessoa>());
    }


    public bool CriarNovaPessoa()
    {
        bool result = false;

        if (Validar() is List<string> erros && erros.Any()) throw new ValidacaoException(string.Join(Environment.NewLine, erros));

        AuditarCriacao();

        Versionar();

        RegistrarEvento(new PessoaCriadaEvent(PessoaId));

        switch (Tipo)
        {
            case TipoPessoaFisica when PessoaFisica != null: PessoaFisica.PessoaFisicaId = PessoaId; return PessoaFisica.CriarNovaPessoaFisica();
            case TipoPessoaJuridica when PessoaJuridica != null: PessoaJuridica.PessoaJuridicaId = PessoaId; return PessoaJuridica.CriarNovaPessoaJuridica();
            default: break;
        }

        return result;
    }

    public bool EditarPessoa(Pessoa pessoa)
    {
        bool result = false;

        if (pessoa is null) throw new ValidacaoException(PessoaNaoPodeSerNula);

        Tipo = pessoa.Tipo;

        if (Validar() is List<string> erros && erros.Any()) throw new ValidacaoException(string.Join(Environment.NewLine, erros));

        AuditarEdicao();

        Versionar();

        RegistrarEvento(new PessoaEditadaEvent(PessoaId));

        switch (Tipo)
        {
            case TipoPessoaFisica when PessoaFisica != null && pessoa.PessoaFisica != null: PessoaJuridica = null; return PessoaFisica.EditarPessoaFisica(pessoa.PessoaFisica);
            case TipoPessoaJuridica when PessoaJuridica != null && pessoa.PessoaJuridica != null: PessoaFisica = null; return PessoaJuridica.EditarPessoaJuridica(pessoa.PessoaJuridica);
            default: break;
        }

        return result;
    }

    public bool RemoverPessoa()
    {
        bool result = false;

        AuditarRemocao();

        Versionar();

        RegistrarEvento(new PessoaRemovidaEvent(PessoaId));

        switch (Tipo)
        {
            case TipoPessoaFisica when PessoaFisica is null: throw new ValidacaoException(PessoaFisicaNaoAssociada);
            case TipoPessoaFisica when PessoaFisica is not null: return PessoaFisica.RemoverPessoaFisica();
            case TipoPessoaJuridica when PessoaJuridica is null: throw new ValidacaoException(PessoaJuridicaNaoAssociada);
            case TipoPessoaJuridica when PessoaJuridica is not null: return PessoaJuridica.RemoverPessoaJuridica();
            default: break;
        }

        return result;
    }

    public bool BloquearPessoa()
    {
        AuditarBloqueio();

        Versionar();

        RegistrarEvento(new PessoaBloqueadaEvent(PessoaId));

        return true;
    }

    public bool DesbloquearPessoa()
    {
        AuditarDesbloqueio();

        Versionar();

        RegistrarEvento(new PessoaDesbloqueadaEvent(PessoaId));

        return true;
    }

    public void AssociarPessoaFisica(PessoaFisica pessoaFisica)
    {
        PessoaFisica = pessoaFisica ?? throw new ArgumentNullException(nameof(pessoaFisica), PessoaFisicaNaoPodeSerNula);
    }

    public void AssociarPessoaJuridica(PessoaJuridica pessoaJuridica)
    {
        PessoaJuridica = pessoaJuridica ?? throw new ArgumentNullException(nameof(pessoaJuridica), PessoaJuridicaNaoPodeSerNula);
    }


    private void AuditarCriacao()
    {
        Controle = new(created: DateTime.UtcNow, modified: null, isRemoved: false, isBlocked: false);
    }

    private void AuditarEdicao()
    {
        Controle = new(created: Controle.Created, modified: DateTime.UtcNow, isRemoved: Controle.IsRemoved, isBlocked: Controle.IsBlocked);
    }

    private void AuditarRemocao()
    {
        Controle = new(created: Controle.Created, modified: DateTime.UtcNow, isRemoved: true, isBlocked: Controle.IsBlocked);
    }

    private void AuditarBloqueio()
    {
        Controle = new(created: Controle.Created, modified: DateTime.UtcNow, isRemoved: Controle.IsRemoved, isBlocked: true);
    }

    private void AuditarDesbloqueio()
    {
        Controle = new(created: Controle.Created, modified: DateTime.UtcNow, isRemoved: Controle.IsRemoved, isBlocked: false);
    }

    private List<string> Validar()
    {
        var validationErrors = new List<string>();
        var roles = new PessoaValidator();
        var resultRoles = roles.Validate(this);
        if (!resultRoles.IsValid)
            validationErrors.AddRange(resultRoles.Errors.Select(e => e.ErrorMessage));
        return validationErrors;
    }

    private void Versionar()
    {
        var versions = Versao.Dados.ToList() ?? new List<Pessoa>();
        versions.Add(this);
        Versao = new Versionamento(dados: new ReadOnlyCollection<Pessoa>(versions));
    }
}
