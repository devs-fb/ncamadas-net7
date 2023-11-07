namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaFisicaCriadaEvent : IEvent
{
    public Guid PessoaFisicaId { get; }

    public PessoaFisicaCriadaEvent(Guid pessoaFisicaId)
    {
        PessoaFisicaId = pessoaFisicaId;
    }
}
