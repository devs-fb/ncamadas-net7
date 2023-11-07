namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaFisicaEditadaEvent : IEvent
{
    public Guid PessoaFisicaId { get; }

    public PessoaFisicaEditadaEvent(Guid pessoaFisicaId)
    {
        PessoaFisicaId = pessoaFisicaId;
    }
}
