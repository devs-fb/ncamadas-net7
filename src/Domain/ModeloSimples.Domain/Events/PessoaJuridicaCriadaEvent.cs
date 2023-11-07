namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaJuridicaCriadaEvent : IEvent
{
    public Guid PessoaJuridicaId { get; }

    public PessoaJuridicaCriadaEvent(Guid pessoaJuridicaId)
    {
        PessoaJuridicaId = pessoaJuridicaId;
    }
}
