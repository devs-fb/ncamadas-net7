namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaJuridicaEditadaEvent : IEvent
{
    public Guid PessoaJuridicaId { get; }

    public PessoaJuridicaEditadaEvent(Guid pessoaJuridicaId)
    {
        PessoaJuridicaId = pessoaJuridicaId;
    }
}
