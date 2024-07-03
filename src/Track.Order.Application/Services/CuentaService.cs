using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
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

public class CuentaService : ICuentaService
{
    private readonly ICuentaRepository _cuentaRepository;
    public CuentaService(ICuentaRepository cuentaRepository)
    {
        _cuentaRepository = cuentaRepository;
    }
    public async Task<string> AgregarCuenta(AgregarCuentaRequest cuenta)
    {

        var nuevacuenta = new Cuenta
        {
            Nombre = cuenta.Nombre
        };

        await _cuentaRepository.AddAsync(nuevacuenta);

        return "Cuenta agregada exitosamente";
    }
    public async Task<List<Cuenta>> GetCuentaAsync()
    {
        var cuentas = await _cuentaRepository.GetAllAsync();
        var cuentasFiltradas = cuentas.Where(cat => !string.IsNullOrEmpty(cat.Nombre)).ToList();


        return cuentas.ToList();
    }

}
