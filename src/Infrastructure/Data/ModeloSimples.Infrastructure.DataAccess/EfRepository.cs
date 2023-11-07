namespace ModeloSimples.Infrastructure.DataAccess;

using ModeloSimples.Infrastructure.Shared.Interfaces;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly PrincipalContext _dbContext;

    public EfRepository(PrincipalContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Remove(entity);
    }

}