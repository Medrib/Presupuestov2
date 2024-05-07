using System.Linq.Expressions;
using Track.Order.Api.Contracts.Order;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Common.Models;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Interfaces;

public interface IOrderService
{
    Task<IturriResult> GetAllOrderAsync();
    Task<IturriResult> SearchOrdersAsync(Filters filters, Sort orderBy,Pagination pagination,bool search, string nombre,string correoElectronico);
    Task<UserRoleResponse> GetRolByIdAsync(string nombre,string correoElectronico);
}
