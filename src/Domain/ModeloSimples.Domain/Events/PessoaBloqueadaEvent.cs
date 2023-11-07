namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaBloqueadaEvent : IEvent
{
    public Guid PessoaId { get; }

    public PessoaBloqueadaEvent(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }
}
