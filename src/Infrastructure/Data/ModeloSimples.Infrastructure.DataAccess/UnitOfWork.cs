namespace ModeloSimples.Infrastructure.DataAccess;

using Microsoft.EntityFrameworkCore.Storage;
using ModeloSimples.Infrastructure.Shared.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly PrincipalContext _dbContext;
    private readonly Dictionary<Type, object> _repositories;
    private readonly IEventBus _eventBus;
    private IDbContextTransaction _transaction;
    
    public UnitOfWork(PrincipalContext dbContext, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _eventBus = eventBus;
        _repositories = new Dictionary<Type, object>();
    }

    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (_repositories.ContainsKey(typeof(TEntity)))
        {
            return _repositories[typeof(TEntity)] as IRepository<TEntity>;
        }

        var repository = new EfRepository<TEntity>(_dbContext);
        _repositories[typeof(TEntity)] = repository;

        return repository;
    }

    public void BeginTransaction()
    {
        _transaction = _dbContext.Database.BeginTransaction();
    }

    public async Task<int> CommitAsync()
    {
        int result = 0;

        try
        {
            result = await _dbContext.SaveChangesAsync();
            _transaction.Commit();

            await ExecuteEventBusAsync();
        }
        catch
        {
            result = 0;
            _transaction.Rollback();
            throw;
        }
        finally
        {
            _transaction.Dispose();
        }

        return result;
    }

    public void Rollback()
    {
        if (_transaction is null) return;
        _transaction.Rollback();
        _transaction.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        var result = await _dbContext.SaveChangesAsync();
        
        await ExecuteEventBusAsync();

        return result;
    }

    private async Task ExecuteEventBusAsync()
    {
        var domainEvents = _dbContext.GetDomainEvents();

        await _eventBus.PublishRange(domainEvents);

        _dbContext.ClearDomainEvents();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}