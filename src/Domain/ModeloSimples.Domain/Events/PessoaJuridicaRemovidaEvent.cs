namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaJuridicaRemovidaEvent : IEvent
{
    public Guid PessoaJuridicaId { get; }

    public PessoaJuridicaRemovidaEvent(Guid pessoaJuridicaId)
    {
        PessoaJuridicaId = pessoaJuridicaId;
    }
}
