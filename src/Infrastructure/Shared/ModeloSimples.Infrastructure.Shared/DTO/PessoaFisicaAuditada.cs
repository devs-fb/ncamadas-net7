namespace ModeloSimples.Infrastructure.Shared.DTO;

/// <summary>
/// Representa os dados de uma Pessoa Física auditada.
/// </summary>
public class PessoaFisicaAuditadaModel
{
    /// <summary>
    /// Obtém ou define o nome social da Pessoa Física auditada.
    /// </summary>
    public string NomeSocial { get; set; }

    /// <summary>
    /// Obtém ou define a data de nascimento da Pessoa Física auditada.
    /// </summary>
    public DateTime DataNascimento { get; set; }

    /// <summary>
    /// Obtém ou define o gênero da Pessoa Física auditada.
    /// </summary>
    public string Genero { get; set; }
}
