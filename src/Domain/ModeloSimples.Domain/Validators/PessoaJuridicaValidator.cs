namespace ModeloSimples.Domain.Validators;

using FluentValidation;
using ModeloSimples.Domain.Entities;

public class PessoaJuridicaValidator : AbstractValidator<PessoaJuridica>
{
    private const string MensagemRazaoSocialVazia = "A razão social não pode estar em branco.";
    private const string MensagemRazaoSocialExcedente = "A razão social não pode ter mais de 250 caracteres.";
    private const string MensagemNomeFantasiaVazio = "O nome fantasia não pode estar em branco.";
    private const string MensagemNomeFantasiaExcedente = "O nome fantasia não pode ter mais de 250 caracteres.";
    private const string MensagemCNAEInvalido = "O CNAE deve ter 7 dígitos.";

    public PessoaJuridicaValidator()
    {
        RuleFor(pessoaJuridica => pessoaJuridica.RazaoSocial)
            .NotEmpty().WithMessage(MensagemRazaoSocialVazia)
            .MaximumLength(250).WithMessage(MensagemRazaoSocialExcedente);

        RuleFor(pessoaJuridica => pessoaJuridica.NomeFantasia)
            .NotEmpty().WithMessage(MensagemNomeFantasiaVazio)
            .MaximumLength(250).WithMessage(MensagemNomeFantasiaExcedente);

        RuleFor(pessoaJuridica => pessoaJuridica.CNAE)
            .Matches(@"^\d{7}$").WithMessage(MensagemCNAEInvalido);
    }
}