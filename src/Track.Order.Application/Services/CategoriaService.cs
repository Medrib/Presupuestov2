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

public class CategoriaService: ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(
        ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;

    }
    public async Task<string> AgregarCategoria(AgregarCategoriaRequest detalle)
    {

        var nuevoCategoria = new CategoriaGasto
        {
            Nombre = detalle.Nombre,
            Descripcion = detalle.Descripcion
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
