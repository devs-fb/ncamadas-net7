namespace ModeloSimples.Domain.ValueObjects;

public sealed class Auditoria
{
    private Auditoria() { }
    public Auditoria(DateTime created, DateTime? modified, bool isRemoved, bool isBlocked)
    {
        Created = created;
        Modified = modified;
        IsRemoved = isRemoved;
        IsBlocked = isBlocked;
    }

    public DateTime Created { get; }
    public DateTime? Modified { get; }
    public bool IsRemoved { get; }
    public bool IsBlocked { get; }
}
