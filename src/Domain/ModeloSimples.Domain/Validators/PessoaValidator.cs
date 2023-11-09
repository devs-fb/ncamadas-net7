namespace ModeloSimples.Domain.Validators;

using FluentValidation;
using ModeloSimples.Domain.Aggregates;

public class PessoaValidator : AbstractValidator<Pessoa>
{
    private const string TipoPessoaFisica = "PessoaFisica";
    private const string TipoPessoaJuridica = "PessoaJuridica";

    private const string MensagemIdVazia = "O ID da pessoa não pode estar em branco.";
    private const string MensagemTipoVazio = "O tipo da pessoa não pode estar em branco.";
    private const string MensagemTipoInvalido = "O tipo da pessoa deve ser 'PessoaFisica' ou 'PessoaJuridica'.";
    private const string MensagemPessoaFisicaNula = "A pessoa física não pode ser nula.";
    private const string MensagemPessoaJuridicaNula = "A pessoa jurídica não pode ser nula.";

    public PessoaValidator()
    {
        RuleFor(pessoa => pessoa.PessoaId)
            .NotEmpty().WithMessage(MensagemIdVazia);

        RuleFor(pessoa => pessoa.Tipo)
            .NotEmpty().WithMessage(MensagemTipoVazio)
            .Must(tipo => tipo.Equals(TipoPessoaFisica, StringComparison.OrdinalIgnoreCase) || tipo.Equals(TipoPessoaJuridica, StringComparison.OrdinalIgnoreCase))
            .WithMessage(MensagemTipoInvalido);

        When(pessoa => pessoa.Tipo.Equals(TipoPessoaFisica, StringComparison.OrdinalIgnoreCase), () => {
            RuleFor(pessoa => pessoa.PessoaFisica)
                .NotNull().WithMessage(MensagemPessoaFisicaNula);
        });

        When(pessoa => pessoa.Tipo.Equals(TipoPessoaJuridica, StringComparison.OrdinalIgnoreCase), () => {
            RuleFor(pessoa => pessoa.PessoaJuridica)
                .NotNull().WithMessage(MensagemPessoaJuridicaNula);
        });
    }
}