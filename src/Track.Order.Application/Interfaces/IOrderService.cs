using System.Linq.Expressions;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Common.Models;
using Track.Order.Domain.Entities;
using Track.Order.Api.Contracts.Ingreso;

namespace Track.Order.Application.Interfaces;

public interface IOrderService
{
    Task<IturriResult> GetAllGastosAsync();
    Task<IturriResult> SearchOrdersAsync(Filters filters, Sort orderBy, Pagination pagination, bool search);
    Task<int> CountOrdersAsync(Filters filters);
    Task<string> AgregarGasto(AgregarGastoRequest detalle);

    Task<string> AgregarIngreso(AgregarIngresoRequest detalle);

    Task<string> AgregarCategoria(AgregarCategoriaRequest categoria);

    Task<List<CategoriaGasto>> GetCategoriesAsync();
}