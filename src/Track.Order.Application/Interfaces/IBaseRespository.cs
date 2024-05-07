using System.Linq.Expressions;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Interfaces;

public interface IBaseRespository<TEntity, TEntityId>
    where TEntity : class
    where TEntityId : struct
{

    Task<IEnumerable<TEntity>> SearchAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "");

    Task<IReadOnlyList<TEntity>> GetAllAsync();

    Task<TEntity?> AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

}
