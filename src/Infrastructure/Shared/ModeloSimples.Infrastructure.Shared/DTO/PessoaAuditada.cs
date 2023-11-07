namespace ModeloSimples.Infrastructure.Shared.DTO;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaAuditadaModel : IAuditoria
{
    public Guid Id { get; set; }
    public string Tipo { get; set; }
    public DateTime Operacao { get; set; }
    public bool Removido { get; set; }
    public bool Bloquado { get; set; }

    public PessoaFisicaAuditadaModel PessoaFisica { get; set; }
    public PessoaJuridicaAuditadaModel PessoaJuridica { get; set; }
}
