namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaDesbloqueadaEvent : IEvent
{
    public Guid PessoaId { get; }

    public PessoaDesbloqueadaEvent(Guid pessoaId)
    {
        PessoaId = pessoaId;
    }
}
