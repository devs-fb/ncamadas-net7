namespace ModeloSimples.Domain.Events;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class PessoaFisicaRemovidaEvent : IEvent
{
    public Guid PessoaFisicaId { get; }

    public PessoaFisicaRemovidaEvent(Guid pessoaFisicaId)
    {
        PessoaFisicaId = pessoaFisicaId;
    }
}
