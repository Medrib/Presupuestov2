﻿using System.Linq.Expressions;
using Track.Order.Api.Contracts;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common.Models;
using Track.Order.Domain.Entities;
using Track.Order.Api.Contracts.Ingreso;
using Track.Order.Api.Contracts.Cuenta;
using Track.Order.Api.Contracts.Usuario;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc;

namespace Track.Order.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    private readonly ICategoriaRepository _categoriaRepository;
    private readonly ICuentaRepository _cuentaRepository;
    private readonly IUsuarioRepository _usuarioRepository;

    public OrderService(IOrderRepository orderRepository,
        IIngresoRepository ingresoRepository, 
        ICategoriaRepository categoriaRepository, 
        ICuentaRepository cuentaRepository,
        IUsuarioRepository usuarioRepository)
    {
        _orderRepository = orderRepository;
        _categoriaRepository = categoriaRepository;
        _cuentaRepository = cuentaRepository;
        _usuarioRepository = usuarioRepository;
    }
    public async Task<IturriResult> GetAllGastosAsync()
    {
        Func<IQueryable<Domain.Entities.Gastos>, IOrderedQueryable<Domain.Entities.Gastos>>? orderBy = null;
        var filters = new Filters();

        Expression<Func<Domain.Entities.Gastos, bool>> filter = BuildFilter(filters);
        var order = await _orderRepository.SearchAsync(filter, orderBy, "CategoriaGasto,Cuenta");

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

            if (orderBy == null)
                return IturriResult.Fail(new Common.Errors.IturriError(null, $"{sort.ColumnName}_column_does_not_exist", System.Net.HttpStatusCode.BadRequest));
        }


        var ordersFiltered = await _orderRepository.SearchAsync(filter, orderBy, "CategoriaGasto,Cuenta");
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
            IDCuenta =detalle.IdCuenta,
            Monto = detalle.Monto,
            Fecha = detalle.Fecha,
            Descripcion = detalle.Descripcion
        };
        await _orderRepository.AddAsync(nuevoGasto);

        return "Gasto agregado exitosamente";
    }

   

    public async Task<string> eliminarGasto(int id)
    {
        try
        { 
            var gasto = await _orderRepository.GetByIdAsync(id);

            if (gasto != null)
            {
                await _orderRepository.DeleteAsync(gasto);
                return "Gasto eliminado exitosamente";
            }
            else
            {
                return "No se encontró ningún gasto con el ID proporcionado";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
    public async Task<string> editarGasto(editarGastoRequest detalle)
    {
        var datos = await _orderRepository.GetByIdAsync(detalle.IDGasto);

        if (datos != null)
        {
            datos.Monto = detalle.Monto;
            datos.Fecha = detalle.Fecha;
            datos.Descripcion = detalle.Descripcion;
            datos.IDCategoriaGasto = detalle.IDCategoriaGasto;
            datos.IDCuenta = detalle.IdCuenta;

            await _orderRepository.UpdateAsync(datos);
            return "Gasto editado exitosamente.";
        }
        else
        {
            return "Ocurrio un problema al editar el gasto.";
        }
    }
    public async Task<List<Usuario>> GetUsuarioAsync()
    {
        var usuario = await _usuarioRepository.GetAllAsync();
        var usuariosObtenidos = usuario.Where(cat => !string.IsNullOrEmpty(cat.Nombre)).ToList();

        return usuario.ToList();
    }

    public async Task<Usuario> loginUser(UsuarioRequest loginRequest)
    {
        var usuario = await _usuarioRepository.SearchAsync(u => u.CorreoElectronico == loginRequest.CorreoElectronico);
        var loginUsuario = usuario.FirstOrDefault();

        if (loginUsuario != null && loginUsuario.Contraseña == loginRequest.Contraseña)
        {
            
            return loginUsuario;
        }
        else
        {
            return null;
        }
    }

    public async Task<string> CreateUsuario(CreateUsuarioRequest detalle)
    {

        var nuevoUsuario = new Usuario
        {
            Nombre = detalle.Nombre,
            CorreoElectronico = detalle.CorreoElectronico,
            Contraseña = detalle.Contraseña,
        };

        await _usuarioRepository.AddAsync(nuevoUsuario);
    
        return "Usuario agregado exitosamente";
    }
    public async Task<string> eliminarUsuario(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);

        try
        {

            if (usuario != null)
            {
                await _usuarioRepository.DeleteAsync(usuario);
                return "usuario eliminado exitosamente";
            }
            else
            {
                return "No se encontró ningún usuario con el ID proporcionado";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
