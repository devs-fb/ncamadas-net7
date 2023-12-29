namespace ModeloSimples.Infrastructure.Shared.DTO;

using ModeloSimples.Infrastructure.Shared.Interfaces;

/// <summary>
/// Representa os dados de uma Pessoa auditada, registrando informações de auditoria.
/// </summary>
public class PessoaAuditadaModel : IAuditoria
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
    /// Obtém ou define a data e hora da operação realizada na Pessoa.
    /// </summary>
    public DateTime Operacao { get; set; }

    /// <summary>
    /// Indica se a Pessoa foi removida.
    /// </summary>
    public bool Removido { get; set; }

    /// <summary>
    /// Indica se a Pessoa está bloqueada.
    /// </summary>
    public bool Bloquado { get; set; }

    /// <summary>
    /// Obtém ou define os detalhes da Pessoa Física auditada.
    /// </summary>
    public PessoaFisicaAuditadaModel PessoaFisica { get; set; }

    /// <summary>
    /// Obtém ou define os detalhes da Pessoa Jurídica auditada.
    /// </summary>
    public PessoaJuridicaAuditadaModel PessoaJuridica { get; set; }
}
