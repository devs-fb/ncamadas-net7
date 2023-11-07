namespace ModeloSimples.Infrastructure.Shared.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
