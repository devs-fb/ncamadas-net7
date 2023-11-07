namespace ModeloSimples.Infrastructure.Shared.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync();
    void BeginTransaction();
    Task<int> CommitAsync();
    void Rollback();
}
