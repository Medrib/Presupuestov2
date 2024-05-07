﻿using System.Linq.Expressions;
using Track.Order.Api.Contracts;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common.Models;
using Track.Order.Domain.Entities;
using Track.Order.Api.Contracts.Ingreso;

namespace Track.Order.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IIngresoRepository _ingresoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public OrderService(IOrderRepository orderRepository,IIngresoRepository ingresoRepository, ICategoriaRepository categoriaRepository)
    {
        _orderRepository = orderRepository;
        _ingresoRepository = ingresoRepository;
        _categoriaRepository = categoriaRepository;
    }
    public async Task<IturriResult> GetAllGastosAsync()
    {
        Func<IQueryable<Domain.Entities.Gastos>, IOrderedQueryable<Domain.Entities.Gastos>>? orderBy = null;
        var filters = new Filters();

        Expression<Func<Domain.Entities.Gastos, bool>> filter = BuildFilter(filters);
        //var order = await _orderRepository.GetAllAsync();
        var order = await _orderRepository.SearchAsync(filter, orderBy, "CategoriaGasto");

        if (order is null)
            return IturriResult.Fail(new Common.Errors.IturriError(null, "order_not_found", System.Net.HttpStatusCode.NotFound));

        return IturriResult.Success(order);
    }

    private IQueryable<Domain.Entities.Gastos> ConvertToQueryable(IEnumerable<Domain.Entities.Gastos> orders)
    {
        return orders.AsQueryable();
    }
    public async Task<IturriResult> SearchOrdersAsync(Filters filters, Sort sort, Pagination pagination, bool search)
    {
        Func<IQueryable<Domain.Entities.Gastos>, IOrderedQueryable<Domain.Entities.Gastos>>? orderBy = null;

        Expression<Func<Domain.Entities.Gastos, bool>> filter = BuildFilter(filters);

        if (!string.IsNullOrEmpty(sort.ColumnName) && !string.IsNullOrEmpty(sort.SortBy))
        {
            orderBy = OrderByColumn(sort.ColumnName!, sort.SortBy!);

            if (orderBy == null) // column name does not exist
                return IturriResult.Fail(new Common.Errors.IturriError(null, $"{sort.ColumnName}_column_does_not_exist", System.Net.HttpStatusCode.BadRequest));
        }


        var ordersFiltered = await _orderRepository.SearchAsync(filter, orderBy, "CategoriaGasto");
        bool allFiltersAreNull = AreAllPropertiesNull(filters);


        // Apply pagination
        var ordersQueryable = ConvertToQueryable(ordersFiltered);
        var pagedOrders = PaginateResults(ordersQueryable, pagination);

        // Build Response
        var response = new DataWithSize<Domain.Entities.Gastos>()
        {
            Data = pagedOrders,
            TotalItems = ordersFiltered.Count()
        };

        return IturriResult.Success(response);
    }

    public bool AreAllPropertiesNull(Filters filters)
    {
        return filters == null || typeof(Filters).GetProperties().All(property => property.GetValue(filters) == null);
    }
    public async Task<int> CountOrdersAsync(Filters filters)
    {
        Expression<Func<Domain.Entities.Gastos, bool>> filter = BuildFilter(filters);

        var ordersFiltered = await _orderRepository.SearchAsync(filter);

        int totalCount = ordersFiltered.Count();

        return totalCount;
    }
    public static Expression<Func<Domain.Entities.Gastos, bool>> BuildFilter(Filters filters)
    {
        Expression<Func<Domain.Entities.Gastos, bool>> filter = x => true;

        if (filters.IDGasto != null)
            filter = CombineFilter(filter, x => x.IDGasto == filters.IDGasto);

        if (filters.Monto != null)
            filter = CombineFilter(filter, x => x.Monto == filters.Monto);

        if (filters.Fecha != null)
            filter = CombineFilter(filter, x => x.Fecha == filters.Fecha);

        if (filters.Descripcion != null)
            filter = CombineFilter(filter, x => x.Descripcion == filters.Descripcion);

        return filter;
    }

    private static Expression<Func<Domain.Entities.Gastos, bool>> CombineFilter(
         Expression<Func<Domain.Entities.Gastos, bool>> existingFilter, Expression<Func<Domain.Entities.Gastos, bool>> newFilter)
    {
        var parameter = Expression.Parameter(typeof(Domain.Entities.Gastos));

        var body = Expression.AndAlso(
            Expression.Invoke(existingFilter, parameter),
            Expression.Invoke(newFilter, parameter));

        return Expression.Lambda<Func<Domain.Entities.Gastos, bool>>(body, parameter);
    }
    private static Func<IQueryable<Domain.Entities.Gastos>, IOrderedQueryable<Domain.Entities.Gastos>>? OrderByColumn(
    string columnName, string sortOrder)
    {
        Func<IQueryable<Domain.Entities.Gastos>, IOrderedQueryable<Domain.Entities.Gastos>>? orderBy = null;

        var property = typeof(Domain.Entities.Gastos).GetProperty(columnName);

        if (property != null)
        {
            var parameter = Expression.Parameter(typeof(Domain.Entities.Gastos));
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);


            var propertyType = property.PropertyType;
            var convertedProperty = Expression.Convert(propertyAccess, typeof(object));


            var funcType = typeof(Func<,>).MakeGenericType(typeof(Domain.Entities.Gastos), typeof(object));
            var orderByExpression = Expression.Lambda(funcType, convertedProperty, parameter);


            if (sortOrder.ToUpper() == "ASC")
                orderBy = q => Queryable.OrderBy(q, (dynamic)orderByExpression);
            else
                orderBy = q => Queryable.OrderByDescending(q, (dynamic)orderByExpression);
        }

        return orderBy;
    }

    private List<Domain.Entities.Gastos> PaginateResults(IQueryable<Domain.Entities.Gastos> orders, Pagination pagination)
    {
        int startIndex = (pagination.PageNumber - 1) * pagination.PageSize;
        int endIndex = startIndex + pagination.PageSize - 1;

        var pagedOrders = orders.Skip(startIndex).Take(pagination.PageSize).ToList();

        return pagedOrders;
    }
    public async Task<string> AgregarGasto(AgregarGastoRequest detalle)
    {
        var nuevoGasto = new Gastos
        {
            IDPresupuesto = detalle.IDPresupuesto,
            IDCategoriaGasto = detalle.IDCategoriaGasto,
            Monto = detalle.Monto,
            Fecha = detalle.Fecha,
            Descripcion = detalle.Descripcion
        };
        await _orderRepository.AddAsync(nuevoGasto);

        return "Gasto agregado exitosamente";
    }

    public async Task<string> AgregarIngreso(AgregarIngresoRequest detalle)
    {
        var nuevoIngreso = new Ingresos
        {
            IDPresupuesto = detalle.IDPresupuesto,
            Monto = detalle.Monto,
            Fecha = detalle.Fecha,
            Descripcion = detalle.Descripcion
        };
        await _ingresoRepository.AddAsync(nuevoIngreso);

        return "Ingreso agregado exitosamente";
    }
    public async Task<string> AgregarCategoria(AgregarCategoriaRequest detalle)
    {

        var nuevoCategoria = new CategoriaGasto
        {
            Nombre = detalle.Nombre,
            Descripcion = null
        };

        await _categoriaRepository.AddAsync(nuevoCategoria);

        return "Categoria agregado exitosamente";
    }
    public async Task<List<CategoriaGasto>> GetCategoriesAsync()
    {
        var categories = await _categoriaRepository.GetAllAsync();
        var filteredCategories = categories.Where(cat => !string.IsNullOrEmpty(cat.Nombre)); 


        return filteredCategories.ToList();
    }

}