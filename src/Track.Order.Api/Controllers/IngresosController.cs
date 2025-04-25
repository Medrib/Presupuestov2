using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common;
using Track.Order.Api.Contracts.Ingreso;
using Track.Order.Api.Contracts.Cuenta;
using Track.Order.Application.Services;

namespace Track.Order.Api.Controllers;

[ApiController]
[Route("/ingresos")]
public class IngresosController : Controller
{
    private readonly IIngresosService _ingresosService;

    public IngresosController(IIngresosService ingresosService)
    {
        _ingresosService = ingresosService;
    }

    [HttpPost("agregarIngreso")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AgregarIngreso([FromBody] AgregarIngresoRequest detalle)
    {
        try
        {
            var serviceResult = await _ingresosService.AgregarIngreso(detalle);
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el ingreso.");
        }
    }
}
