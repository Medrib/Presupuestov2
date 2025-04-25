using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Application.Interfaces;

namespace Track.Order.Api.Controllers;

[ApiController]
[Route("/categoria")]
public class CategoriaController : Controller
{
    public readonly ICategoriaService _categoriaService;


    public CategoriaController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet("getCategory")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetCategoriesAsync()
    {
        try
        {
            var serviceResult = await _categoriaService.GetCategoriesAsync();
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al realizar el GetCategory.");
        }

    }
    [HttpPost("agregarCategoria")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AgregarCategoria([FromBody] AgregarCategoriaRequest categoria)
    {
        try
        {
            var serviceResult = await _categoriaService.AgregarCategoria(categoria);
            return Ok(serviceResult);
        }
        catch (Exception)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el categoria.");
        }
    }
}
