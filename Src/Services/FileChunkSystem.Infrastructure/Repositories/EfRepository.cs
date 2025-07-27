using FileChunkSystem.Application.Abstractions.Repositories;
using FileChunkSystem.Domain.Common;
using FileChunkSystem.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FileChunkSystem.Infrastructure.Repositories;
public class EfRepository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, bool tracking = false, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbSet;

        if (!tracking) query = query.AsNoTracking();

        return await query.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken);

    public async Task<List<T>> ListAsync(int skip = 0, int take = 100, CancellationToken ct = default)
        => await _dbSet.AsNoTracking().OrderByDescending(e => e.CreatedAt).Skip(skip).Take(take).ToListAsync(ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _dbSet.AddRangeAsync(entities, cancellationToken);

    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is not null)
        {
            entity.IsActive = false;

            _dbSet.Update(entity);
        }

        return Task.CompletedTask;
    }

    public Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.IsActive = false;
        }

        _dbSet.UpdateRange(entities);

        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(e => e.Id == id, cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public DbSet<T> Set() => _dbSet;
}