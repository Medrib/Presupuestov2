namespace Track.Order.Application.Interfaces;

using System.Linq.Expressions;
using Track.Order.Api.Contracts.Order;
using Track.Order.Domain.Entities;
public interface IOrderRepository : IBaseRespository<Orders, int>
{
    Task<UserRoleResponse> GetRolByIdAsync(string nombre,string correoElectronico);
    Task<IQueryable<Orders>> GetOrdersByIdAsync(int idUsuario);
    Task<IEnumerable<Orders>> OrdersFiltered(
        IQueryable<Orders> data,
        Expression<Func<Orders, bool>>? filter,
        Func<IQueryable<Orders>, IOrderedQueryable<Orders>>? orderBy,
        string includeProperties = "");
}
