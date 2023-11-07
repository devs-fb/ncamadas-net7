namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaCriadaEvent : IEvent
{
    public Guid PessoaId { get; }

    public PessoaCriadaEvent(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }
}
