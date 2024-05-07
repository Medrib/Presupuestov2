namespace Track.Order.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

public class BaseRepository<TEntity, TEntityId> : IBaseRespository<TEntity, TEntityId>
    where TEntity : class
    where TEntityId : struct
{
    public BaseRepository(TrackOrderDbContext dbContext)
    => DbContext = dbContext;

    protected TrackOrderDbContext DbContext { get; }

    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id)
        => await DbContext.Set<TEntity>().FindAsync(id);

    public virtual async Task<IReadOnlyList<TEntity>> GetAllAsync()
        => await DbContext.Set<TEntity>().ToListAsync();

    public virtual async Task<TEntity?> AddAsync(TEntity entity)
    {
        await DbContext.Set<TEntity>().AddAsync(entity);
        await DbContext.SaveChangesAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbContext.ChangeTracker.Clear();
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> SearchAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = DbContext.Set<TEntity>();

        if (filter != null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(
            new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        else
            return await query.ToListAsync();
    }
}
