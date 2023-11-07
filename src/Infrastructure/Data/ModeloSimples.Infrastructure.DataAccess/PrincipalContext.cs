namespace ModeloSimples.Infrastructure.DataAccess;

using Microsoft.EntityFrameworkCore;
using ModeloSimples.Domain.Aggregates;
using ModeloSimples.Domain.Base;
using ModeloSimples.Domain.Entities;
using ModeloSimples.Infrastructure.DataAccess.Mappings;
using ModeloSimples.Infrastructure.Shared.Interfaces;
using System.Linq;

public class PrincipalContext : DbContext
{
    private readonly List<IEvent> _pendingDomainEvents = new();

    public PrincipalContext(DbContextOptions<PrincipalContext> options) : base(options) { }

    public DbSet<Pessoa> Pessoa { get; set; }
    public DbSet<PessoaFisica> PessoaFisica { get; set; }
    public DbSet<PessoaJuridica> PessoaJuridica { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var savedChanges = await base.SaveChangesAsync(cancellationToken);

        ReleaseEvents();

        return savedChanges;
    }

    public IReadOnlyList<IEvent> GetDomainEvents()
    {
        return _pendingDomainEvents.AsReadOnly();
    }

    public void ClearDomainEvents()
    {
        _pendingDomainEvents.Clear();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Ignore<DomainEvent>();

        _ = new PessoaMapping(builder.Entity<Pessoa>());
    }

    private IList<IEvent> FindUnifyEvents()
    {
        var domainEvents = ChangeTracker.Entries()
            .Where(entry => entry.Entity is AggregateRoot || entry.Entity is Entity)
            .SelectMany(entry =>
                entry.Entity switch
                {
                    AggregateRoot aggregateRoot => aggregateRoot.DomainEvents,
                    Entity entity => entity.DomainEvents,
                    _ => Enumerable.Empty<IEvent>() 
                })
            .Distinct()
            .ToList();

        return domainEvents;
    }

    private void ReleaseEvents()
    {
        var domainEvents = FindUnifyEvents();
        if (domainEvents.Any())
        {
            foreach (var domainEvent in domainEvents)
            {
                _pendingDomainEvents.Add(domainEvent);
            }
        }
    }
}
