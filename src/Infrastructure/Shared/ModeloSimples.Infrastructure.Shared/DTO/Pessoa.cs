namespace ModeloSimples.Infrastructure.Shared.DTO;

using System.Text.Json.Serialization;

/// <summary>
/// Representa uma entidade Pessoa, que pode ser tanto uma Pessoa Física quanto uma Pessoa Jurídica.
/// </summary>
public class PessoaModel
{
    /// <summary>
    /// Obtém ou define o identificador único da Pessoa.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtém ou define o tipo da Pessoa (Física ou Jurídica).
    /// </summary>
    public string Tipo { get; set; }

    /// <summary>
    /// Obtém ou define os detalhes da Pessoa Física. Será ignorado na serialização se for o valor padrão.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PessoaFisicaModel PessoaFisica { get; set; } = null;

    /// <summary>
    /// Obtém ou define os detalhes da Pessoa Jurídica. Será ignorado na serialização se for o valor padrão.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public PessoaJuridicaModel PessoaJuridica { get; set; } = null;
}

/// <summary>
/// Representa os dados de uma Pessoa Física.
/// </summary>
public class PessoaFisicaModel
{
    /// <summary>
    /// Obtém ou define o nome social da Pessoa Física.
    /// </summary>
    public string NomeSocial { get; set; }

    /// <summary>
    /// Obtém ou define a data de nascimento da Pessoa Física.
    /// </summary>
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Obtém ou define o gênero da Pessoa Física.
    /// </summary>
    public string Genero { get; set; }
}

/// <summary>
/// Representa os dados de uma Pessoa Jurídica.
/// </summary>
public class PessoaJuridicaModel
{
    /// <summary>
    /// Obtém ou define a razão social da Pessoa Jurídica.
    /// </summary>
    public string RazaoSocial { get; set; }

    /// <summary>
    /// Obtém ou define o nome fantasia da Pessoa Jurídica.
    /// </summary>
    public string NomeFantasia { get; set; }

    /// <summary>
    /// Obtém ou define o Código Nacional de Atividade Econômica (CNAE) da Pessoa Jurídica.
    /// </summary>
    public string CNAE { get; set; }
}