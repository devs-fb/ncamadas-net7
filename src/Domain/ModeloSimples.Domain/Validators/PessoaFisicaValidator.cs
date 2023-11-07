namespace ModeloSimples.Domain.Validators;

using FluentValidation;
using ModeloSimples.Domain.Entities;

public class PessoaFisicaValidator : AbstractValidator<PessoaFisica>
{
    private const string GeneroMasculino = "M";
    private const string GeneroFeminino = "F";
    private const string GeneroOutros = "O";

    private const string MensagemNomeSocialVazio = "O nome social não pode estar em branco.";
    private const string MensagemNomeSocialExcedente = "O nome social não pode ter mais de 100 caracteres.";
    private const string MensagemDataNascimentoVazia = "A data de nascimento não pode estar em branco.";
    private const string MensagemDataNascimentoInvalida = "A data de nascimento deve ser anterior à data atual.";
    private const string MensagemGeneroVazio = "O gênero não pode estar em branco.";
    private const string MensagemGeneroInvalido = "O gênero deve ser 'M' para 'Masculino', 'F' para 'Feminino' ou 'O' para 'Outro'.";

    public PessoaFisicaValidator()
    {
        RuleFor(pessoaFisica => pessoaFisica.NomeSocial)
            .NotEmpty().WithMessage(MensagemNomeSocialVazio)
            .MaximumLength(250).WithMessage(MensagemNomeSocialExcedente);

        RuleFor(pessoaFisica => pessoaFisica.DataNascimento)
            .NotEmpty().WithMessage(MensagemDataNascimentoVazia)
            .LessThan(DateTime.Now).WithMessage(MensagemDataNascimentoInvalida);

        RuleFor(pessoaFisica => pessoaFisica.Genero)
            .NotEmpty().WithMessage(MensagemGeneroVazio)
            .Must(genero => genero.Equals(GeneroMasculino, StringComparison.OrdinalIgnoreCase)
            || genero.Equals(GeneroFeminino, StringComparison.OrdinalIgnoreCase)
            || genero.Equals(GeneroOutros, StringComparison.OrdinalIgnoreCase))
            .WithMessage(MensagemGeneroInvalido);
    }
}
