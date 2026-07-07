using System.Linq.Expressions;
using Gym.Domain.Interfaces;
using Gym.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace Gym.Application.Tests;

public class TestRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet;

    public TestRepository(DbSet<T> dbSet) => _dbSet = dbSet;

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) => _dbSet.FindAsync(new object[] { id }, ct).AsTask()!;
    public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default) => _dbSet.ToListAsync(ct).ContinueWith(t => (IReadOnlyList<T>)t.Result, ct);
    public Task<IReadOnlyList<T>> GetAllAsync(int page, int pageSize, CancellationToken ct = default) => _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<T>)t.Result, ct);
    public Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => _dbSet.Where(predicate).ToListAsync(ct).ContinueWith(t => (IReadOnlyList<T>)t.Result, ct);
    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => _dbSet.FirstOrDefaultAsync(predicate, ct).ContinueWith(t => t.Result, ct);
    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) => _dbSet.AnyAsync(predicate, ct);
    public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default) => predicate == null ? _dbSet.CountAsync(ct) : _dbSet.CountAsync(predicate, ct);
    public Task<T> AddAsync(T entity, CancellationToken ct = default) { _dbSet.Add(entity); return Task.FromResult(entity); }
    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) { _dbSet.AddRange(entities); return Task.CompletedTask; }
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
    public void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
    public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default) => throw new NotImplementedException();
    public Task<T?> FirstOrDefaultAsync(ISpecification<T> spec, CancellationToken ct = default) => throw new NotImplementedException();
    public Task<int> CountAsync(ISpecification<T> spec, CancellationToken ct = default) => throw new NotImplementedException();
}
