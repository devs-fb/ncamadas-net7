namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaEditadaEvent : IEvent
{
    public Guid PessoaId { get; }

    public PessoaEditadaEvent(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }
}
