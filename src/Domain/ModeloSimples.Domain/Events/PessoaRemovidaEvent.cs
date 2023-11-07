namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaRemovidaEvent : IEvent
{
    public Guid PessoaId { get; }

    public PessoaRemovidaEvent(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }
}