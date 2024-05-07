using System.Linq.Expressions;
using Track.Order.Api.Contracts;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common.Models;
using Track.Order.Api.Contracts.Enums;
using Track.Order.Api.Contracts.Order;

namespace Track.Order.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<IturriResult> GetAllOrderAsync()
    {
        var order = await _orderRepository.GetAllAsync();

        if (order is null)
            return IturriResult.Fail(new Common.Errors.IturriError(null, "order_not_found", System.Net.HttpStatusCode.NotFound));

        return IturriResult.Success(order);
    }

    private IQueryable<Domain.Entities.Orders> ConvertToQueryable(IEnumerable<Domain.Entities.Orders> orders)
    {
        return orders.AsQueryable();
    }
    public async Task<IturriResult> SearchOrdersAsync(Filters filters, Sort sort, Pagination pagination, bool search , string nombre, string correoElectronico)
    {
        var userRole = await GetRolByIdAsync(nombre,correoElectronico);
        filters.IdUsuario = userRole.IdUsuario;
        if (userRole.Rol == (int)Roles.Rol.gestorContrato || userRole.Rol == (int)Roles.Rol.coordinador)
        {
            filters.IdUsuario = null;
        }
        IEnumerable<Domain.Entities.Orders>? ordersFiltered = null;
        Func<IQueryable<Domain.Entities.Orders>, IOrderedQueryable<Domain.Entities.Orders>>? orderBy = null;
        Expression<Func<Domain.Entities.Orders, bool>>? filter = BuildFilterBasedOnSearch(filters, search);

        if (!string.IsNullOrEmpty(sort.ColumnName) && !string.IsNullOrEmpty(sort.SortBy))
        {
            orderBy = OrderByColumn(sort.ColumnName!, sort.SortBy!);

            if (orderBy == null) // column name does not exist
                return IturriResult.Fail(new Common.Errors.IturriError(null, $"{sort.ColumnName}_column_does_not_exist", System.Net.HttpStatusCode.BadRequest));
        }

        ordersFiltered = await GetFilteredOrdersAsync(userRole.Rol, userRole.IdUsuario, filter, orderBy);
        bool allFiltersAreNull = AreAllPropertiesNull(filters);
        // Apply pagination
        var pagedOrders = PaginateOrders(ordersFiltered, pagination, allFiltersAreNull);

        // Build Response
        var response = new DataWithSize<Domain.Entities.Orders>()
        {
            Data = pagedOrders,
            TotalItems = ordersFiltered.Count(),
            UserRole = userRole
        };
        return IturriResult.Success(response);
    }

    private List<Domain.Entities.Orders> PaginateOrders(IEnumerable<Domain.Entities.Orders> ordersFiltered, Pagination pagination, bool allFiltersAreNull)
    {
    var ordersQueryable = ConvertToQueryable(ordersFiltered);
    var pagedOrders = PaginateResults(ordersQueryable, pagination, allFiltersAreNull);
    return pagedOrders;
    }

    private async Task<IEnumerable<Domain.Entities.Orders>> GetFilteredOrdersAsync(int userRole, int idUsuario, Expression<Func<Domain.Entities.Orders, bool>> filter, Func<IQueryable<Domain.Entities.Orders>, IOrderedQueryable<Domain.Entities.Orders>>? orderBy)
    {
        IEnumerable<Domain.Entities.Orders> ordersFiltered;

        if (userRole == (int)Roles.Rol.coordinador)
        {
            var ordersByCoordinator = await _orderRepository.GetOrdersByIdAsync(idUsuario);
            ordersFiltered = await _orderRepository.OrdersFiltered(ordersByCoordinator, filter, orderBy, "Estados,Usuarios");
        }
        else
        {
            ordersFiltered = await _orderRepository.SearchAsync(filter, orderBy, "Estados,Usuarios");
        }

        return ordersFiltered;
    }

    private Expression<Func<Domain.Entities.Orders, bool>> BuildFilterBasedOnSearch(Filters filters, bool search)
    {
        Expression<Func<Domain.Entities.Orders, bool>> filter;

        if (search)
        {
            filter = BuildFilter(filters);
        }
        else
        {
            filter = BuildFilterContains(filters);
        }

        return filter;
    }
    public bool AreAllPropertiesNull(Filters filters)
    {
        return filters == null || typeof(Filters).GetProperties().All(property => property.GetValue(filters) == null);
    }
    public static Expression<Func<Domain.Entities.Orders, bool>> BuildFilter(Filters filters)
    {
        Expression<Func<Domain.Entities.Orders, bool>> filter = x => true;

        if (filters.Id != null)
            filter = CombineFilter(filter, x => x.Id == filters.Id);

        if (filters.ContratoSAP != null)
            filter = CombineFilter(filter, x => x.ContratoSAP == filters.ContratoSAP);

        if (filters.Estado != null)
            filter = CombineFilter(filter, x => x.Estados.Descripcion == filters.Estado);

        if (filters.IdUsuario != null)
            filter = CombineFilter(filter, x => x.IdUsuario == filters.IdUsuario);

        if (filters.PedidoCliente != null)
            filter = CombineFilter(filter, x => x.PedidoCliente == filters.PedidoCliente);

        if (filters.CreadoEl != null)
            filter = CombineFilter(filter, x => x.CreadoEl == filters.CreadoEl);

        if (filters.FechaPedidoCliente != null)
            filter = CombineFilter(filter, x => x.FechaPedidoCliente == filters.FechaPedidoCliente);

        if (filters.ClaseDocVentas != null)
            filter = CombineFilter(filter, x => x.ClaseDocVentas == filters.ClaseDocVentas);

        if (filters.Pedido != null)
            filter = CombineFilter(filter, x => x.Pedido == filters.Pedido);

        if (filters.PosPed != null)
            filter = CombineFilter(filter, x => x.PosPed == filters.PosPed);

        if (filters.Solicitante != null)
            filter = CombineFilter(filter, x => x.Solicitante == filters.Solicitante);

        if (filters.SolDesc != null)
            filter = CombineFilter(filter, x => x.SolDesc == filters.SolDesc);

        if (filters.Pagador != null)
            filter = CombineFilter(filter, x => x.Pagador == filters.Pagador);

        if (filters.DescPagador != null)
            filter = CombineFilter(filter, x => x.DescPagador == filters.DescPagador);

        if (filters.DMDesc != null)
            filter = CombineFilter(filter, x => x.DMDesc == filters.DMDesc);

        if (filters.RefEmpleado != null)
            filter = CombineFilter(filter, x => x.RefEmpleado == filters.RefEmpleado);

        if (filters.Almacen != null)
            filter = CombineFilter(filter, x => x.Almacen == filters.Almacen);

        if (filters.Material != null)
            filter = CombineFilter(filter, x => x.Material == filters.Material);

        if (filters.TextoBreveMaterial != null)
            filter = CombineFilter(filter, x => x.TextoBreveMaterial == filters.TextoBreveMaterial);

        if (filters.NumeroMaterialCliente != null)
            filter = CombineFilter(filter, x => x.NumeroMaterialCliente == filters.NumeroMaterialCliente);

        if (filters.FechaPrimerReparto != null)
            filter = CombineFilter(filter, x => x.FechaPrimerReparto == filters.FechaPrimerReparto);

        if (filters.CantidadPedido != null)
            filter = CombineFilter(filter, x => x.CantidadPedido == filters.CantidadPedido);

        if (filters.CtdPend != null)
            filter = CombineFilter(filter, x => x.CtdPend == filters.CtdPend);

        if (filters.CtdEntreg != null)
            filter = CombineFilter(filter, x => x.CtdEntreg == filters.CtdEntreg);

        if (filters.UnMedidaVenta != null)
            filter = CombineFilter(filter, x => x.UnMedidaVenta == filters.UnMedidaVenta);

        if (filters.Entrega != null)
            filter = CombineFilter(filter, x => x.Entrega == filters.Entrega);

        if (filters.Factura != null)
            filter = CombineFilter(filter, x => x.Factura == filters.Factura);

        if (filters.Referencia != null)
            filter = CombineFilter(filter, x => x.Referencia == filters.Referencia);

        if (filters.NroTransporte != null)
            filter = CombineFilter(filter, x => x.NroTransporte == filters.NroTransporte);

        if (filters.FechaConfirmacion != null)
            filter = CombineFilter(filter, x => x.FechaConfirmacion == filters.FechaConfirmacion);

        if (filters.NumIDExt != null)
            filter = CombineFilter(filter, x => x.NumIDExt == filters.NumIDExt);

        if (filters.Oportunidad != null)
            filter = CombineFilter(filter, x => x.Oportunidad == filters.Oportunidad);

        if (filters.ClasePedido != null)
            filter = CombineFilter(filter, x => x.ClasePedido == filters.ClasePedido);

        if (filters.MotivoPedido != null)
            filter = CombineFilter(filter, x => x.MotivoPedido == filters.MotivoPedido);

        if (filters.NExpCliente != null)
            filter = CombineFilter(filter, x => x.NExpCliente == filters.NExpCliente);

        if (filters.NumContCliente != null)
            filter = CombineFilter(filter, x => x.NumContCliente == filters.NumContCliente);

        if (filters.DireccionEmail != null)
            filter = CombineFilter(filter, x => x.DireccionEmail == filters.DireccionEmail);

        if (filters.Lote != null)
            filter = CombineFilter(filter, x => x.Lote == filters.Lote);

        if (filters.UrlTransportista != null)
            filter = CombineFilter(filter, x => x.UrlTransportista == filters.UrlTransportista);

        if (filters.DocumentoAnexo != null)
            filter = CombineFilter(filter, x => x.DocumentoAnexo == filters.DocumentoAnexo);

        if (filters.DestinatarioMercancias != null)
            filter = CombineFilter(filter, x => x.DestinatarioMercancias == filters.DestinatarioMercancias);



        return filter;
    }

    public static Expression<Func<Domain.Entities.Orders, bool>> BuildFilterContains(Filters filters)
    {
        Expression<Func<Domain.Entities.Orders, bool>> filter = x => true;

        if (filters.Id != null)
            filter = CombineFilter(filter, x => x.Id == filters.Id);

        if (filters.ContratoSAP != null)
        {
            string? contratoSAPString = filters.ContratoSAP.ToString();
            filter = CombineFilter(filter, x => x.ContratoSAP.ToString().Contains(contratoSAPString));
        }

        if (filters.Estado != null)
        {
            string idEstadoString = filters.Estado.ToString();
            filter = CombineFilter(filter, x => x.Estados.Descripcion.Contains(idEstadoString));
        }

        if (filters.IdUsuario != null)
        {
            string? idUsuarioString = filters.IdUsuario.ToString();
            filter = CombineFilter(filter, x => x.IdUsuario.ToString().Contains(idUsuarioString));
        }

        if (!string.IsNullOrEmpty(filters.PedidoCliente))
        {
            filter = CombineFilter(filter, x => x.PedidoCliente.Contains(filters.PedidoCliente));
        }

        if (filters.CreadoEl != null)
            filter = CombineFilter(filter, x => x.CreadoEl == filters.CreadoEl);

        if (filters.FechaPedidoCliente != null)
            filter = CombineFilter(filter, x => x.FechaPedidoCliente == filters.FechaPedidoCliente);

        if (!string.IsNullOrEmpty(filters.ClaseDocVentas))
        {
            filter = CombineFilter(filter, x => x.ClaseDocVentas.Contains(filters.ClaseDocVentas));
        }

        if (filters.Pedido != null)
        {
            string? pedidoString = filters.Pedido.ToString();
            filter = CombineFilter(filter, x => x.Pedido.ToString().Contains(pedidoString));
        }

        if (filters.PosPed != null)
        {
            string? posPedString = filters.PosPed.ToString();
            filter = CombineFilter(filter, x => x.PosPed.ToString().Contains(posPedString));
        }

        if (filters.Solicitante != null)
        {
            string? solicitanteString = filters.Solicitante.ToString();
            filter = CombineFilter(filter, x => x.Solicitante.ToString().Contains(solicitanteString));
        }

        if (!string.IsNullOrEmpty(filters.SolDesc))
        {
            filter = CombineFilter(filter, x => x.SolDesc.Contains(filters.SolDesc));
        }
        if (filters.Pagador != null)
        {
            string? pagadorString = filters.Pagador.ToString();
            filter = CombineFilter(filter, x => x.Pagador.ToString().Contains(pagadorString));
        }

        if (!string.IsNullOrEmpty(filters.DescPagador))
        {
            filter = CombineFilter(filter, x => x.DescPagador.Contains(filters.DescPagador));
        }

        if (!string.IsNullOrEmpty(filters.DMDesc))
        {
            filter = CombineFilter(filter, x => x.DMDesc.Contains(filters.DMDesc));
        }

        if (filters.RefEmpleado != null)
        {
            string? refEmpleadoString = filters.RefEmpleado.ToString();
            filter = CombineFilter(filter, x => x.RefEmpleado.ToString().Contains(refEmpleadoString));
        }

        if (filters.Almacen != null)
        {
            string? almacenString = filters.Almacen.ToString();
            filter = CombineFilter(filter, x => x.Almacen.ToString().Contains(almacenString));
        }

        if (!string.IsNullOrEmpty(filters.Material))
        {
            filter = CombineFilter(filter, x => x.Material.Contains(filters.Material));
        }

        if (!string.IsNullOrEmpty(filters.TextoBreveMaterial))
        {
            filter = CombineFilter(filter, x => x.TextoBreveMaterial.Contains(filters.TextoBreveMaterial));
        }

        if (!string.IsNullOrEmpty(filters.NumeroMaterialCliente))
        {
            filter = CombineFilter(filter, x => x.NumeroMaterialCliente.Contains(filters.NumeroMaterialCliente));
        }

        if (filters.FechaPrimerReparto != null)
            filter = CombineFilter(filter, x => x.FechaPrimerReparto == filters.FechaPrimerReparto);

        if (filters.CantidadPedido != null)
        {
            string? cantidadPedidoString = filters.CantidadPedido.ToString();
            filter = CombineFilter(filter, x => x.CantidadPedido.ToString().Contains(cantidadPedidoString));
        }

        if (filters.CtdPend != null)
        {
            string? ctdPendString = filters.CtdPend.ToString();
            filter = CombineFilter(filter, x => x.CtdPend.ToString().Contains(ctdPendString));
        }

        if (filters.CtdEntreg != null)
        {
            string? ctdEntregString = filters.CtdEntreg.ToString();
            filter = CombineFilter(filter, x => x.CtdEntreg.ToString().Contains(ctdEntregString));
        }

        if (!string.IsNullOrEmpty(filters.UnMedidaVenta))
        {
            filter = CombineFilter(filter, x => x.UnMedidaVenta.Contains(filters.UnMedidaVenta));
        }

        if (filters.Entrega != null)
        {
            string? entregaString = filters.Entrega.ToString();
            filter = CombineFilter(filter, x => x.Entrega.ToString().Contains(entregaString));
        }

        if (filters.Factura != null)
        {
            string? facturaString = filters.Factura.ToString();
            filter = CombineFilter(filter, x => x.Factura.ToString().Contains(facturaString));
        }

        if (filters.Referencia != null)
        {
            string? referenciaString = filters.Referencia.ToString();
            filter = CombineFilter(filter, x => x.Referencia.ToString().Contains(referenciaString));
        }

        if (filters.NroTransporte != null)
        {
            string? nroTransporteString = filters.NroTransporte.ToString();
            filter = CombineFilter(filter, x => x.NroTransporte.ToString().Contains(nroTransporteString));
        }

        if (filters.FechaConfirmacion != null)
        {
            filter = CombineFilter(filter, x => x.FechaConfirmacion == filters.FechaConfirmacion);
        }

        if (filters.NumIDExt != null)
        {
            string? numIDExtString = filters.NumIDExt.ToString();
            filter = CombineFilter(filter, x => x.NumIDExt.ToString().Contains(numIDExtString));
        }

        if (!string.IsNullOrEmpty(filters.Oportunidad))
        {
            filter = CombineFilter(filter, x => x.Oportunidad.Contains(filters.Oportunidad));
        }

        if (!string.IsNullOrEmpty(filters.ClasePedido))
        {
            filter = CombineFilter(filter, x => x.ClasePedido.Contains(filters.ClasePedido));
        }

        if (!string.IsNullOrEmpty(filters.MotivoPedido))
        {
            filter = CombineFilter(filter, x => x.MotivoPedido.Contains(filters.MotivoPedido));
        }

        if (filters.NExpCliente != null)
        {
            string? nExpClienteString = filters.NExpCliente.ToString();
            filter = CombineFilter(filter, x => x.NExpCliente.ToString().Contains(nExpClienteString));
        }

        if (!string.IsNullOrEmpty(filters.NumContCliente))
        {
            filter = CombineFilter(filter, x => x.NumContCliente.Contains(filters.NumContCliente));
        }

        if (!string.IsNullOrEmpty(filters.DireccionEmail))
        {
            filter = CombineFilter(filter, x => x.DireccionEmail.Contains(filters.DireccionEmail));
        }

        if (!string.IsNullOrEmpty(filters.Lote))
        {
            filter = CombineFilter(filter, x => x.Lote.Contains(filters.Lote));
        }

        if (!string.IsNullOrEmpty(filters.UrlTransportista))
        {
            filter = CombineFilter(filter, x => x.UrlTransportista.Contains(filters.UrlTransportista));
        }

        if (!string.IsNullOrEmpty(filters.DocumentoAnexo))
        {
            filter = CombineFilter(filter, x => x.DocumentoAnexo.Contains(filters.DocumentoAnexo));
        }

        if (filters.DestinatarioMercancias != null)
        {
            string? destinatarioMercanciasString = filters.DestinatarioMercancias.ToString();
            filter = CombineFilter(filter, x => x.DestinatarioMercancias.ToString().Contains(destinatarioMercanciasString));
        }
        return filter;
    }

    private static Expression<Func<Domain.Entities.Orders, bool>> CombineFilter(
         Expression<Func<Domain.Entities.Orders, bool>> existingFilter, Expression<Func<Domain.Entities.Orders, bool>> newFilter)
    {
        var parameter = Expression.Parameter(typeof(Domain.Entities.Orders));

        var body = Expression.AndAlso(
            Expression.Invoke(existingFilter, parameter),
            Expression.Invoke(newFilter, parameter));

        return Expression.Lambda<Func<Domain.Entities.Orders, bool>>(body, parameter);
    }
    private static Func<IQueryable<Domain.Entities.Orders>, IOrderedQueryable<Domain.Entities.Orders>>? OrderByColumn(
    string columnName, string sortOrder)
    {
        Func<IQueryable<Domain.Entities.Orders>, IOrderedQueryable<Domain.Entities.Orders>>? orderBy = null;

        var property = typeof(Domain.Entities.Orders).GetProperty(columnName);

        if (property != null)
        {
            var parameter = Expression.Parameter(typeof(Domain.Entities.Orders));
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);


            var propertyType = property.PropertyType;
            var convertedProperty = Expression.Convert(propertyAccess, typeof(object));


            var funcType = typeof(Func<,>).MakeGenericType(typeof(Domain.Entities.Orders), typeof(object));
            var orderByExpression = Expression.Lambda(funcType, convertedProperty, parameter);


            if (sortOrder.ToUpper() == "ASC")
                orderBy = q => Queryable.OrderBy(q, (dynamic)orderByExpression);
            else
                orderBy = q => Queryable.OrderByDescending(q, (dynamic)orderByExpression);
        }

        return orderBy;
    }

    private List<Domain.Entities.Orders> PaginateResults(IQueryable<Domain.Entities.Orders> orders, Pagination pagination, bool allFiltersAreNull)
    {
        int startIndex = (pagination.PageNumber - 1) * pagination.PageSize;
        int pageSize = pagination.PageSize;
        if (allFiltersAreNull)
        {
            return orders.Skip(startIndex).Take(pageSize).ToList();
        }
        else
        {
            return orders.ToList();
        }
    }

    public async Task<UserRoleResponse> GetRolByIdAsync(string nombre,string correoElectronico)
    {

        var userRole = await _orderRepository.GetRolByIdAsync(nombre, correoElectronico);
        
        return userRole;
    }

}

