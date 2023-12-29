namespace ModeloSimples.Infrastructure.Shared.DTO;

/// <summary>
/// Representa os dados de uma Pessoa Jurídica auditada.
/// </summary>
public class PessoaJuridicaAuditadaModel
{
    /// <summary>
    /// Obtém ou define a razão social da Pessoa Jurídica auditada.
    /// </summary>
    public string RazaoSocial { get; set; }

    /// <summary>
    /// Obtém ou define o nome fantasia da Pessoa Jurídica auditada.
    /// </summary>
    public string NomeFantasia { get; set; }

    /// <summary>
    /// Obtém ou define o Código Nacional de Atividade Econômica (CNAE) da Pessoa Jurídica auditada.
    /// </summary>
    public string CNAE { get; set; }
}